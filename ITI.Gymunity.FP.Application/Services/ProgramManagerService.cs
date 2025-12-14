using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.Program;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
 }

 public class ProgramManagerService : IProgramManagerService
 {
 private readonly IProgramRepository _repo;
 private readonly IUnitOfWork _unitOfWork;
 private readonly IMapper _mapper;

 public ProgramManagerService(IProgramRepository repo, IUnitOfWork unitOfWork, IMapper mapper)
 {
 _repo = repo;
 _unitOfWork = unitOfWork;
 _mapper = mapper;
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
 var entity = new Program
 {
 TrainerId = request.TrainerId,
 Title = request.Title,
 Description = request.Description,
 Type = request.Type,
 DurationWeeks = request.DurationWeeks,
 Price = request.Price,
 // Newly created programs should be pending by default until admin approves
 IsPublic = false,
 MaxClients = request.MaxClients,
 ThumbnailUrl = request.ThumbnailUrl
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
 }
}
