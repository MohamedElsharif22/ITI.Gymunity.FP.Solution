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
 public interface IDayExerciseService
 {
 Task<IReadOnlyList<ProgramDayExerciseGetAllResponse>> GetByDayAsync(int dayId);
 Task<ProgramDayExerciseGetAllResponse?> GetByIdAsync(int id);
 Task<ProgramDayExerciseGetAllResponse> CreateAsync(ProgramDayExerciseGetAllResponse request);
 Task<bool> UpdateAsync(int id, ProgramDayExerciseGetAllResponse request);
 Task<bool> DeleteAsync(int id);
 }

 public class DayExerciseService : IDayExerciseService
 {
 private readonly IDayExerciseRepository _repo;
 private readonly IUnitOfWork _unitOfWork;
 private readonly IMapper _mapper;

 public DayExerciseService(IDayExerciseRepository repo, IUnitOfWork unitOfWork, IMapper mapper)
 {
 _repo = repo;
 _unitOfWork = unitOfWork;
 _mapper = mapper;
 }

 public async Task<IReadOnlyList<ProgramDayExerciseGetAllResponse>> GetByDayAsync(int dayId)
 {
 var list = await _repo.GetByDayIdAsync(dayId);
 return list.Select(e => _mapper.Map<ProgramDayExerciseGetAllResponse>(e)).ToList();
 }

 public async Task<ProgramDayExerciseGetAllResponse?> GetByIdAsync(int id)
 {
 var e = await _repo.GetByIdAsync(id);
 if (e == null) return null;
 return _mapper.Map<ProgramDayExerciseGetAllResponse>(e);
 }

 public async Task<ProgramDayExerciseGetAllResponse> CreateAsync(ProgramDayExerciseGetAllResponse request)
 {
 var entity = new ProgramDayExercise
 {
 ProgramDayId = request.ProgramDayId,
 ExerciseId = request.ExerciseId,
 OrderIndex = request.OrderIndex,
 Sets = request.Sets,
 Reps = request.Reps,
 RestSeconds = request.RestSeconds,
 Tempo = request.Tempo,
 RPE = request.RPE,
 Percent1RM = request.Percent1RM,
 Notes = request.Notes,
 VideoUrl = request.VideoUrl,
 ExerciseDataJson = request.ExerciseDataJson
 };
 _repo.Add(entity);
 await _unitOfWork.CompleteAsync();
 return _mapper.Map<ProgramDayExerciseGetAllResponse>(entity);
 }

 public async Task<bool> UpdateAsync(int id, ProgramDayExerciseGetAllResponse request)
 {
 var entity = await _repo.GetByIdAsync(id);
 if (entity == null) return false;
 entity.OrderIndex = request.OrderIndex;
 entity.Sets = request.Sets;
 entity.Reps = request.Reps;
 entity.RestSeconds = request.RestSeconds;
 entity.Tempo = request.Tempo;
 entity.RPE = request.RPE;
 entity.Percent1RM = request.Percent1RM;
 entity.Notes = request.Notes;
 entity.VideoUrl = request.VideoUrl;
 entity.ExerciseDataJson = request.ExerciseDataJson;
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
 }
}
