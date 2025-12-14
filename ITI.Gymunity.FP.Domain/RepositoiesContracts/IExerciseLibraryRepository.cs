using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Domain.RepositoiesContracts
{
 public interface IExerciseLibraryRepository : IRepository<Exercise>
 {
 Task<IReadOnlyList<Exercise>> SearchByNameAsync(string? name, string? trainerId = null);
 }
}
