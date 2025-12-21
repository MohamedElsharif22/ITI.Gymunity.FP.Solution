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
 public interface IDayService
 {
 Task<IReadOnlyList<ProgramDayGetAllResponse>> GetByWeekAsync(int weekId);
 Task<ProgramDayGetAllResponse?> GetByIdAsync(int id);
 Task<ProgramDayGetAllResponse> CreateAsync(ProgramDayGetAllResponse request);
 Task<bool> UpdateAsync(int id, ProgramDayGetAllResponse request);
 Task<bool> DeleteAsync(int id);
 }

 public class DayService : IDayService
 {
 private readonly IDayRepository _repo;
 private readonly IUnitOfWork _unitOfWork;
 private readonly IMapper _mapper;

 public DayService(IDayRepository repo, IUnitOfWork unitOfWork, IMapper mapper)
 {
 _repo = repo;
 _unitOfWork = unitOfWork;
 _mapper = mapper;
 }

 public async Task<IReadOnlyList<ProgramDayGetAllResponse>> GetByWeekAsync(int weekId)
 {
 var list = await _repo.GetByWeekIdAsync(weekId);
 return list.Select(d => _mapper.Map<ProgramDayGetAllResponse>(d)).ToList();
 }

 public async Task<ProgramDayGetAllResponse?> GetByIdAsync(int id)
 {
 var d = await _repo.GetWithExercisesAsync(id);
 if (d == null) return null;
 return _mapper.Map<ProgramDayGetAllResponse>(d);
 }

 public async Task<ProgramDayGetAllResponse> CreateAsync(ProgramDayGetAllResponse request)
 {
 var entity = new ProgramDay { ProgramWeekId = request.ProgramWeekId, DayNumber = request.DayNumber, Title = request.Title, Notes = request.Notes };
 _repo.Add(entity);
 await _unitOfWork.CompleteAsync();
 return _mapper.Map<ProgramDayGetAllResponse>(entity);
 }

 public async Task<bool> UpdateAsync(int id, ProgramDayGetAllResponse request)
 {
 var entity = await _repo.GetByIdAsync(id);
 if (entity == null) return false;
 entity.DayNumber = request.DayNumber;
 entity.Title = request.Title;
 entity.Notes = request.Notes;
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
