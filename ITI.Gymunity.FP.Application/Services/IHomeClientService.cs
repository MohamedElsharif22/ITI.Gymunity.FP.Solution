using ITI.Gymunity.FP.Application.DTOs.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Services
{
 public interface IHomeClientService
 {
 Task<(IReadOnlyList<ProgramClientResponse> programs, IReadOnlyList<TrainerClientResponse> trainers)> SearchAsync(string term);
 Task<IReadOnlyList<ProgramClientResponse>> GetAllProgramsAsync();
 Task<ProgramClientResponse?> GetProgramByIdAsync(int id);
 Task<IReadOnlyList<TrainerClientResponse>> GetAllTrainersAsync();
 Task<TrainerClientResponse?> GetTrainerByIdAsync(int id);

 // Package client endpoints
 Task<IReadOnlyList<PackageClientResponse>> GetAllPackagesAsync();
 Task<PackageClientResponse?> GetPackageByIdAsync(int id);
 Task<IReadOnlyList<PackageClientResponse>> GetPackagesByTrainerIdAsync(int trainerProfileId);
 }
}
