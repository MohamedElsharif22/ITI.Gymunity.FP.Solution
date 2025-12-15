using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.Client;
using ITI.Gymunity.FP.Application.Specefications;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainProgram = ITI.Gymunity.FP.Domain.Models.ProgramAggregate.Program;

namespace ITI.Gymunity.FP.Application.Services
{
 public class HomeClientService : IHomeClientService
 {
 private readonly IUnitOfWork _unitOfWork;
 private readonly IMapper _mapper;

 public HomeClientService(IUnitOfWork unitOfWork, IMapper mapper)
 {
 _unitOfWork = unitOfWork;
 _mapper = mapper;
 }

 public async Task<(IReadOnlyList<ProgramClientResponse> programs, IReadOnlyList<TrainerClientResponse> trainers)> SearchAsync(string term)
 {
 var programSpec = new ProgramWithTrainerSpec(term);
 var programs = await _unitOfWork.Repository<DomainProgram>().GetAllWithSpecsAsync(programSpec);
 var programDtos = programs.Select(p => _mapper.Map<ProgramClientResponse>(p)).ToList();

 var trainerSpec = new TrainerWithUsersAndProgramsSpecs(tp => tp.Handle.Contains(term) || tp.User.FullName.Contains(term));
 var trainers = await _unitOfWork.Repository<TrainerProfile>().GetAllWithSpecsAsync(trainerSpec);
 var trainerDtos = trainers.Select(t => _mapper.Map<TrainerClientResponse>(t)).ToList();

 return (programDtos, trainerDtos);
 }

 public async Task<IReadOnlyList<ProgramClientResponse>> GetAllProgramsAsync()
 {
 var spec = new ProgramWithTrainerSpec();
 var programs = await _unitOfWork.Repository<DomainProgram>().GetAllWithSpecsAsync(spec);
 return programs.Select(p => _mapper.Map<ProgramClientResponse>(p)).ToList();
 }

 public async Task<ProgramClientResponse?> GetProgramByIdAsync(int id)
 {
 // ProgramWithTrainerSpec only accepts optional searchTerm string in constructor.
 // Create default spec and set Criteria to filter by id.
 var specById = new ProgramWithTrainerSpec();
 specById.Criteria = p => p.Id == id;
 var program = (await _unitOfWork.Repository<DomainProgram>().GetAllWithSpecsAsync(specById)).FirstOrDefault();
 if (program == null) return null;
 return _mapper.Map<ProgramClientResponse>(program);
 }

 public async Task<IReadOnlyList<TrainerClientResponse>> GetAllTrainersAsync()
 {
 var spec = new TrainerWithUsersAndProgramsSpecs();
 var trainers = await _unitOfWork.Repository<TrainerProfile>().GetAllWithSpecsAsync(spec);
 return trainers.Select(t => _mapper.Map<TrainerClientResponse>(t)).ToList();
 }

 public async Task<TrainerClientResponse?> GetTrainerByIdAsync(int id)
 {
 var spec = new TrainerWithUsersAndProgramsSpecs(tp => tp.Id == id);
 var trainer = await _unitOfWork.Repository<TrainerProfile>().GetWithSpecsAsync(spec);
 if (trainer == null) return null;
 return _mapper.Map<TrainerClientResponse>(trainer);
 }

 // packages
 public async Task<IReadOnlyList<PackageClientResponse>> GetAllPackagesAsync()
 {
 var list = await _unitOfWork.Repository<Package>().GetAllAsync();
 var active = list.Where(p => p.IsActive).ToList();
 return active.Select(p => _mapper.Map<PackageClientResponse>(p)).ToList();
 }

 public async Task<PackageClientResponse?> GetPackageByIdAsync(int id)
 {
 var p = await _unitOfWork.Repository<Package>().GetByIdAsync(id);
 if (p == null) return null;
 return _mapper.Map<PackageClientResponse>(p);
 }

 public async Task<IReadOnlyList<PackageClientResponse>> GetPackagesByTrainerIdAsync(int trainerProfileId)
 {
 var trainer = await _unitOfWork.Repository<TrainerProfile>().GetByIdAsync(trainerProfileId);
 if (trainer == null) return new List<PackageClientResponse>();
 var list = await _unitOfWork.Repository<Package>().GetAllAsync();
 var byTrainer = list.Where(p => p.TrainerId == trainer.UserId).ToList();
 return byTrainer.Select(p => _mapper.Map<PackageClientResponse>(p)).ToList();
 }
 }
}
