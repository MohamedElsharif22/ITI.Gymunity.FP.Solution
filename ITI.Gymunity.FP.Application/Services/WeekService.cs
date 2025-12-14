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
 public interface IWeekService
 {
 Task<IReadOnlyList<ProgramWeekGetAllResponse>> GetByProgramAsync(int programId);
 Task<ProgramWeekGetAllResponse?> GetByIdAsync(int id);
 Task<ProgramWeekGetAllResponse> CreateAsync(ProgramWeekGetAllResponse request);
 Task<bool> UpdateAsync(int id, ProgramWeekGetAllResponse request);
 Task<bool> DeleteAsync(int id);
 }

 public class WeekService : IWeekService
 {
 private readonly IWeekRepository _repo;
 private readonly IUnitOfWork _unitOfWork;
 private readonly IMapper _mapper;

 public WeekService(IWeekRepository repo, IUnitOfWork unitOfWork, IMapper mapper)
 {
 _repo = repo;
 _unitOfWork = unitOfWork;
 _mapper = mapper;
 }

 public async Task<IReadOnlyList<ProgramWeekGetAllResponse>> GetByProgramAsync(int programId)
 {
 var list = await _repo.GetByProgramIdAsync(programId);
 return list.Select(w => _mapper.Map<ProgramWeekGetAllResponse>(w)).ToList();
 }

 public async Task<ProgramWeekGetAllResponse?> GetByIdAsync(int id)
 {
 var w = await _repo.GetWithDaysAsync(id);
 if (w == null) return null;
 return _mapper.Map<ProgramWeekGetAllResponse>(w);
 }

 public async Task<ProgramWeekGetAllResponse> CreateAsync(ProgramWeekGetAllResponse request)
 {
 var entity = new ProgramWeek { ProgramId = request.ProgramId, WeekNumber = request.WeekNumber };
 _repo.Add(entity);
 await _unitOfWork.CompleteAsync();
 return _mapper.Map<ProgramWeekGetAllResponse>(entity);
 }

 public async Task<bool> UpdateAsync(int id, ProgramWeekGetAllResponse request)
 {
 var entity = await _repo.GetByIdAsync(id);
 if (entity == null) return false;
 entity.WeekNumber = request.WeekNumber;
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
