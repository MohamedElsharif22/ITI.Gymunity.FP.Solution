using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Services
{
 public interface IPackageService
 {
 Task<IReadOnlyList<PackageResponse>> GetAllForTrainerAsync(string trainerId);
 Task<PackageResponse?> GetByIdAsync(int id);
 Task<PackageResponse> CreateAsync(string trainerId, PackageCreateRequest request);
 Task<bool> UpdateAsync(int id, PackageCreateRequest request);
 Task<bool> DeleteAsync(int id);
 Task<bool> ToggleActiveAsync(int id);
 Task<IReadOnlyList<PackageResponse>> GetAllAsync();
 }

 public class PackageService : IPackageService
 {
 private readonly IUnitOfWork _unitOfWork;
 private readonly IMapper _mapper;

 public PackageService(IUnitOfWork unitOfWork, IMapper mapper)
 {
 _unitOfWork = unitOfWork;
 _mapper = mapper;
 }

 public async Task<IReadOnlyList<PackageResponse>> GetAllForTrainerAsync(string trainerId)
 {
 var repo = _unitOfWork.Repository<Package, ITI.Gymunity.FP.Domain.RepositoiesContracts.IPackageRepository>();
 var list = await repo.GetByTrainerIdAsync(trainerId);
 return list.Select(p => _mapper.Map<PackageResponse>(p)).ToList();
 }

 public async Task<PackageResponse?> GetByIdAsync(int id)
 {
 var repo = _unitOfWork.Repository<Package, ITI.Gymunity.FP.Domain.RepositoiesContracts.IPackageRepository>();
 var p = await repo.GetByIdWithProgramsAsync(id);
 if (p == null) return null;
 return _mapper.Map<PackageResponse>(p);
 }

 public async Task<PackageResponse> CreateAsync(string trainerId, PackageCreateRequest request)
 {
 var entity = new Package
 {
 Name = request.Name,
 Description = request.Description,
 PriceMonthly = request.PriceMonthly,
 PriceYearly = request.PriceYearly,
 IsActive = request.IsActive,
 ThumbnailUrl = request.ThumbnailUrl,
 TrainerId = trainerId
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

 return _mapper.Map<PackageResponse>(entity);
 }

 public async Task<bool> UpdateAsync(int id, PackageCreateRequest request)
 {
 var repo = _unitOfWork.Repository<Package>();
 var entity = await repo.GetByIdAsync(id);
 if (entity == null) return false;
 entity.Name = request.Name;
 entity.Description = request.Description;
 entity.PriceMonthly = request.PriceMonthly;
 entity.PriceYearly = request.PriceYearly;
 entity.IsActive = request.IsActive;
 entity.ThumbnailUrl = request.ThumbnailUrl;
 repo.Update(entity);
 await _unitOfWork.CompleteAsync();

 // sync package programs: remove existing, add provided
 var existing = (await _unitOfWork.Repository<PackageProgram>().GetAllAsync()).Where(x => x.PackageId == id).ToList();
 foreach (var ex in existing) _unitOfWork.Repository<PackageProgram>().Delete(ex);
 await _unitOfWork.CompleteAsync();
 if (request.ProgramIds != null && request.ProgramIds.Length >0)
 {
 foreach (var pid in request.ProgramIds)
 {
 _unitOfWork.Repository<PackageProgram>().Add(new PackageProgram { PackageId = id, ProgramId = pid });
 }
 await _unitOfWork.CompleteAsync();
 }

 return true;
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
 return list.Select(p => _mapper.Map<PackageResponse>(p)).ToList();
 }
 }
}
