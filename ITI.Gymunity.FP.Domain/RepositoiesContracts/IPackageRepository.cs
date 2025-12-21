using ITI.Gymunity.FP.Domain.Models.Trainer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Domain.RepositoiesContracts
{
 public interface IPackageRepository : IRepository<Package>
 {
 Task<IReadOnlyList<Package>> GetByTrainerIdAsync(string trainerId);
 Task<IReadOnlyList<Package>> GetAllActiveWithProgramsAsync();
 Task<Package?> GetByIdWithProgramsAsync(int id);
 }
}
