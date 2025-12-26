using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.Program;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ITI.Gymunity.FP.Domain.Models.Identity;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using System;

namespace ITI.Gymunity.FP.Application.Services
{
 public interface IProgramManagerService
 {
 Task<IReadOnlyList<ProgramGetAllResponse>> GetAllAsync();
 Task<ProgramGetByIdResponse?> GetByIdAsync(int id);
 Task<ProgramGetByIdResponse> CreateAsync(ProgramCreateRequest request);
 Task<bool> UpdateAsync(int id, ProgramUpdateRequest request);
 Task<bool> DeleteAsync(int id);
 Task<IReadOnlyList<ProgramGetAllResponse>> SearchAsync(string? term);
 Task<IReadOnlyList<ProgramGetAllResponse>> GetByTrainerAsync(string trainerId);
 }

 public class ProgramManagerService : IProgramManagerService
 {
 private readonly IProgramRepository _repo;
 private readonly IUnitOfWork _unitOfWork;
 private readonly IMapper _mapper;
 private readonly ITrainerProfileRepository _trainerRepo;
 private readonly UserManager<AppUser> _userManager;

 public ProgramManagerService(IProgramRepository repo, IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager, ITrainerProfileRepository trainerRepo)
 {
 _repo = repo;
 _unitOfWork = unitOfWork;
 _mapper = mapper;
 _userManager = userManager;
 _trainerRepo = trainerRepo;
 }

 public async Task<IReadOnlyList<ProgramGetAllResponse>> GetAllAsync()
 {
 var list = await _repo.GetAllAsync();
 return list.Select(p => _mapper.Map<ProgramGetAllResponse>(p)).ToList();
 }

 public async Task<ProgramGetByIdResponse?> GetByIdAsync(int id)
 {
 var p = await _repo.GetByIdWithIncludesAsync(id);
 if (p == null) return null;
 return _mapper.Map<ProgramGetByIdResponse>(p);
 }

 public async Task<ProgramGetByIdResponse> CreateAsync(ProgramCreateRequest request)
 {
 // Validate trainer profile exists
 var profile = await _trainerRepo.GetByIdAsync(request.TrainerProfileId);
 if (profile == null)
 {
 throw new InvalidOperationException($"Trainer profile with id {request.TrainerProfileId} not found.");
 }

 // Global duplicate title check using DB-side query
 if (!string.IsNullOrWhiteSpace(request.Title))
 {
 if (await _repo.ExistsByTitleAsync(request.Title.Trim()))
 {
 throw new InvalidOperationException($"A program with title '{request.Title}' already exists.");
 }
 }

 // Ensure we have a non-null trainer user id for legacy column
 var trainerUserId = profile.UserId ?? string.Empty;

 var entity = new Program
 {
 TrainerProfileId = request.TrainerProfileId,
 TrainerProfile = profile,
 TrainerId = trainerUserId,
 Title = request.Title,
 Description = request.Description,
 Type = request.Type,
 DurationWeeks = request.DurationWeeks,
 Price = request.Price,
 IsPublic = request.IsPublic,
 MaxClients = request.MaxClients,
 ThumbnailUrl = request.ThumbnailUrl,
 CreatedAt = DateTime.UtcNow,
 UpdatedAt = DateTime.UtcNow
 };

 _repo.Add(entity);
 await _unitOfWork.CompleteAsync();
 return _mapper.Map<ProgramGetByIdResponse>(entity);
 }

 public async Task<bool> UpdateAsync(int id, ProgramUpdateRequest request)
 {
 var entity = await _repo.GetByIdAsync(id);
 if (entity == null) return false;
 entity.Title = request.Title;
 entity.Description = request.Description;
 entity.Type = request.Type;
 entity.DurationWeeks = request.DurationWeeks;
 entity.Price = request.Price;
 entity.IsPublic = request.IsPublic;
 entity.MaxClients = request.MaxClients;
 entity.ThumbnailUrl = request.ThumbnailUrl;
 entity.UpdatedAt = DateTime.UtcNow;

 // if TrainerProfileId changed in future requests, update legacy TrainerId accordingly (not part of current DTO)
 _repo.Update(entity);
 await _unitOfWork.CompleteAsync();
 return true;
 }

 public async Task<bool> DeleteAsync(int id)
 {
 var entity = await _repo.GetByIdAsync(id);
 if (entity == null) return false;
 _repo.Delete(entity);
 await _unitOfWork.CompleteAsync();
 return true;
 }

 public async Task<IReadOnlyList<ProgramGetAllResponse>> SearchAsync(string? term)
 {
 var list = await _repo.SearchAsync(term);
 return list.Select(p => _mapper.Map<ProgramGetAllResponse>(p)).ToList();
 }

 public async Task<IReadOnlyList<ProgramGetAllResponse>> GetByTrainerAsync(string trainerId)
 {
 if (int.TryParse(trainerId, out var profileId))
 {
 var list = await _repo.GetByTrainerAsyncProfileId(profileId);
 return list.Select(p => _mapper.Map<ProgramGetAllResponse>(p)).ToList();
 }
 else
 {
 var list = await _repo.GetByTrainerAsync(trainerId);
 return list.Select(p => _mapper.Map<ProgramGetAllResponse>(p)).ToList();
 }
 }
 }
}
