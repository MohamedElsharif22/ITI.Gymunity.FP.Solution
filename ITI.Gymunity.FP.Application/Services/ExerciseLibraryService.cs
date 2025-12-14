using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.ExerciseLibrary;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Services
{
 public interface IExerciseLibraryService
 {
 Task<IReadOnlyList<ExerciseGetAllResponse>> GetAllAsync(string? trainerId = null);
 Task<ExerciseGetByIdResponse?> GetByIdAsync(int id);
 Task<ExerciseGetByIdResponse> CreateAsync(ExerciseCreateRequest request);
 Task<bool> UpdateAsync(int id, ExerciseUpdateRequest request);
 Task<bool> DeleteAsync(int id);
 Task<IReadOnlyList<ExerciseGetAllResponse>> SearchByNameAsync(string? name, string? trainerId = null);
 }

 public class ExerciseLibraryService : IExerciseLibraryService
 {
 private readonly IExerciseLibraryRepository _repo;
 private readonly IUnitOfWork _unitOfWork;
 private readonly IMapper _mapper;

 public ExerciseLibraryService(IExerciseLibraryRepository repo, IUnitOfWork unitOfWork, IMapper mapper)
 {
 _repo = repo;
 _unitOfWork = unitOfWork;
 _mapper = mapper;
 }

 public async Task<IReadOnlyList<ExerciseGetAllResponse>> GetAllAsync(string? trainerId = null)
 {
 var list = await _repo.SearchByNameAsync(null, trainerId);
 return list.Select(e => _mapper.Map<ExerciseGetAllResponse>(e)).ToList();
 }

 public async Task<ExerciseGetByIdResponse?> GetByIdAsync(int id)
 {
 var item = await _repo.GetByIdAsync(id);
 if (item == null) return null;
 return _mapper.Map<ExerciseGetByIdResponse>(item);
 }

 public async Task<ExerciseGetByIdResponse> CreateAsync(ExerciseCreateRequest request)
 {
 var entity = new Exercise
 {
 Name = request.Name,
 Category = request.Category,
 MuscleGroup = request.MuscleGroup,
 Equipment = request.Equipment,
 VideoDemoUrl = request.VideoDemoUrl,
 ThumbnailUrl = request.ThumbnailUrl,
 IsCustom = request.IsCustom,
 TrainerId = request.TrainerId
 };
 _repo.Add(entity);
 await _unitOfWork.CompleteAsync();
 return _mapper.Map<ExerciseGetByIdResponse>(entity);
 }

 public async Task<bool> UpdateAsync(int id, ExerciseUpdateRequest request)
 {
 var entity = await _repo.GetByIdAsync(id);
 if (entity == null) return false;
 entity.Name = request.Name;
 entity.Category = request.Category;
 entity.MuscleGroup = request.MuscleGroup;
 entity.Equipment = request.Equipment;
 entity.VideoDemoUrl = request.VideoDemoUrl;
 entity.ThumbnailUrl = request.ThumbnailUrl;
 entity.IsCustom = request.IsCustom;
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

 public async Task<IReadOnlyList<ExerciseGetAllResponse>> SearchByNameAsync(string? name, string? trainerId = null)
 {
 var list = await _repo.SearchByNameAsync(name, trainerId);
 return list.Select(e => _mapper.Map<ExerciseGetAllResponse>(e)).ToList();
 }
 }
}
