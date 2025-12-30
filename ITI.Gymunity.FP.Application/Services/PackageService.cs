using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.Gymunity.FP.Application.Contracts.ExternalServices;
using ITI.Gymunity.FP.Domain.Models.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace ITI.Gymunity.FP.Application.Services
{
 public interface IPackageService
 {
 Task<IReadOnlyList<PackageResponse>> GetAllForTrainerAsync(int trainerId);
 Task<PackageResponse?> GetByIdAsync(int id);
 Task<PackageResponse> CreateAsync(int trainerId, PackageCreateRequest request);
 Task<bool> UpdateAsync(int id, PackageCreateRequest request);
 Task<bool> DeleteAsync(int id);
 Task<bool> ToggleActiveAsync(int id);
 Task<IReadOnlyList<PackageResponse>> GetAllAsync();
 }

 public class PackageService : IPackageService
 {
 private readonly IUnitOfWork _unitOfWork;
 private readonly IMapper _mapper;
 private readonly IImageUrlResolver _imageResolver;

 public PackageService(IUnitOfWork unitOfWork, IMapper mapper, IImageUrlResolver imageResolver)
 {
 _unitOfWork = unitOfWork;
 _mapper = mapper;
 _imageResolver = imageResolver;
 }

 public async Task<IReadOnlyList<PackageResponse>> GetAllForTrainerAsync(int trainerId)
 {
 var repo = _unitOfWork.Repository<Package, ITI.Gymunity.FP.Domain.RepositoiesContracts.IPackageRepository>();
 var list = await repo.GetByTrainerIdAsync(trainerId);
 var mapped = list.Select(p => _mapper.Map<PackageResponse>(p)).ToList();

 // Resolve thumbnail urls to absolute
 foreach (var pkg in mapped)
 {
 if (!string.IsNullOrWhiteSpace(pkg.ThumbnailUrl))
 pkg.ThumbnailUrl = _imageResolver.ResolveImageUrl(pkg.ThumbnailUrl);
 }

 return mapped;
 }

 public async Task<PackageResponse?> GetByIdAsync(int id)
 {
 var repo = _unitOfWork.Repository<Package, ITI.Gymunity.FP.Domain.RepositoiesContracts.IPackageRepository>();
 var p = await repo.GetByIdWithProgramsAsync(id);
 if (p == null) return null;
 var mapped = _mapper.Map<PackageResponse>(p);
 if (!string.IsNullOrWhiteSpace(mapped.ThumbnailUrl))
 mapped.ThumbnailUrl = _imageResolver.ResolveImageUrl(mapped.ThumbnailUrl);
 return mapped;
 }

 public async Task<PackageResponse> CreateAsync(int trainerId, PackageCreateRequest request)
 {
 // validate trainer profile exists
 var profileRepo = _unitOfWork.Repository<TrainerProfile, ITI.Gymunity.FP.Domain.RepositoiesContracts.ITrainerProfileRepository>();
 var profile = await profileRepo.GetByIdAsync(trainerId);
 if (profile == null)
 {
 throw new InvalidOperationException($"Trainer profile with id {trainerId} not found.");
 }

 var entity = new Package
 {
 Name = request.Name,
 Description = request.Description,
 PriceMonthly = request.PriceMonthly,
 PriceYearly = request.PriceYearly,
 IsActive = request.IsActive,
 ThumbnailUrl = request.ThumbnailUrl,
 TrainerId = trainerId,
 //IsAnnual = request.IsAnnual,
 PromoCode = request.PromoCode ?? string.Empty
 };

 _unitOfWork.Repository<Package>().Add(entity);
 await _unitOfWork.CompleteAsync();

 // sync programs
 if (request.ProgramIds != null && request.ProgramIds.Length >0)
 {
 foreach (var pid in request.ProgramIds)
 {
 var pp = new PackageProgram { PackageId = entity.Id, ProgramId = pid };
 _unitOfWork.Repository<PackageProgram>().Add(pp);
 }
 await _unitOfWork.CompleteAsync();
 }

 var mapped = _mapper.Map<PackageResponse>(entity);
 if (!string.IsNullOrWhiteSpace(mapped.ThumbnailUrl))
 mapped.ThumbnailUrl = _imageResolver.ResolveImageUrl(mapped.ThumbnailUrl);

 return mapped;
 }

 public async Task<bool> UpdateAsync(int id, PackageCreateRequest request)
 {
 try
 {
 // load package with related PackagePrograms to sync correctly
 var packageRepo = _unitOfWork.Repository<Package, ITI.Gymunity.FP.Domain.RepositoiesContracts.IPackageRepository>();
 var entity = await packageRepo.GetByIdWithProgramsAsync(id);
 if (entity == null) return false;

 entity.Name = request.Name;
 entity.Description = request.Description;
 entity.PriceMonthly = request.PriceMonthly;
 entity.PriceYearly = request.PriceYearly;
 entity.IsActive = request.IsActive;
 entity.ThumbnailUrl = request.ThumbnailUrl;
 //entity.IsAnnual = request.IsAnnual;
 entity.PromoCode = request.PromoCode ?? entity.PromoCode ?? string.Empty;

 // use loaded navigation collection to avoid duplicates
 var existingProgramLinks = entity.PackagePrograms?.Where(pp => !pp.IsDeleted).ToList() ?? new List<PackageProgram>();
 var existingProgramIds = existingProgramLinks.Select(x => x.ProgramId).ToHashSet();
 var requestedIds = request.ProgramIds != null ? request.ProgramIds.ToHashSet() : new HashSet<int>();

 var packageProgramRepo = _unitOfWork.Repository<PackageProgram>();

 // remove links that are not requested
 var toRemove = existingProgramLinks.Where(ep => !requestedIds.Contains(ep.ProgramId)).ToList();
 foreach (var ex in toRemove)
 {
 packageProgramRepo.Delete(ex);
 }

 // add links that don't exist yet (handle soft-deleted ones by reviving)
 var allPackagePrograms = (await packageProgramRepo.GetAllAsync()).Where(pp => pp.PackageId == id).ToList();
 foreach (var pid in requestedIds.Where(pid => !existingProgramIds.Contains(pid)))
 {
 var existingRow = allPackagePrograms.FirstOrDefault(pp => pp.ProgramId == pid);
 if (existingRow != null)
 {
 if (existingRow.IsDeleted)
 {
 existingRow.IsDeleted = false;
 packageProgramRepo.Update(existingRow);
 }
 // if exists and not deleted, nothing to do
 }
 else
 {
 packageProgramRepo.Add(new PackageProgram { PackageId = id, ProgramId = pid });
 }
 }

 // update package entity
 _unitOfWork.Repository<Package>().Update(entity);
 await _unitOfWork.CompleteAsync();

 return true;
 }
 catch (DbUpdateException dbEx)
 {
 // wrap to provide clearer message for controller/middleware
 throw new InvalidOperationException("Database update conflict while updating package. Possible duplicate program link.", dbEx);
 }
 }

 public async Task<bool> DeleteAsync(int id)
 {
 var repo = _unitOfWork.Repository<Package>();
 var entity = await repo.GetByIdAsync(id);
 if (entity == null) return false;
 repo.Delete(entity);
 await _unitOfWork.CompleteAsync();
 return true;
 }

 public async Task<bool> ToggleActiveAsync(int id)
 {
 var repo = _unitOfWork.Repository<Package>();
 var entity = await repo.GetByIdAsync(id);
 if (entity == null) return false;
 entity.IsActive = !entity.IsActive;
 repo.Update(entity);
 await _unitOfWork.CompleteAsync();
 return true;
 }

 public async Task<IReadOnlyList<PackageResponse>> GetAllAsync()
 {
 var repo = _unitOfWork.Repository<Package>();
 var list = await repo.GetAllAsync();
 var mapped = list.Select(p => _mapper.Map<PackageResponse>(p)).ToList();

 foreach (var pkg in mapped)
 {
 if (!string.IsNullOrWhiteSpace(pkg.ThumbnailUrl))
 pkg.ThumbnailUrl = _imageResolver.ResolveImageUrl(pkg.ThumbnailUrl);
 }

 return mapped;
 }
 }
}
