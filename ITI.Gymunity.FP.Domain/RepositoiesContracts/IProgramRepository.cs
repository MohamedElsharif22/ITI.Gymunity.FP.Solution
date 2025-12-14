using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Domain.RepositoiesContracts
{
 public interface IProgramRepository : IRepository<Program>
 {
 Task<IReadOnlyList<Program>> GetByTrainerAsync(string trainerId);
 Task<Program?> GetByIdWithIncludesAsync(int id);
 Task<IReadOnlyList<Program>> SearchAsync(string? term);
 }
}
