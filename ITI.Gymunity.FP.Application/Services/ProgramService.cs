using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.Program;
using ITI.Gymunity.FP.Application.Specefications;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ITI.Gymunity.FP.Application.Services
{
 public interface IProgramService
 {
 Task<IReadOnlyList<ProgramResponse>> GetAllProgramsAsync(string? searchTerm = null);
 }

 public class ProgramService : IProgramService
 {
 private readonly IUnitOfWork _unitOfWork;
 private readonly IMapper _mapper;

 public ProgramService(IUnitOfWork unitOfWork, IMapper mapper)
 {
 _unitOfWork = unitOfWork;
 _mapper = mapper;
 }

 public async Task<IReadOnlyList<ProgramResponse>> GetAllProgramsAsync(string? searchTerm = null)
 {
 var spec = new ProgramWithTrainerSpec(searchTerm);
 var programs = await _unitOfWork.Repository<Program>().GetAllWithSpecsAsync(spec);
 var response = _mapper.Map<List<ProgramResponse>>(programs);
 return response;
 }
 }
}
