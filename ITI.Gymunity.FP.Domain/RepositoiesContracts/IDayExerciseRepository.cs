using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Domain.RepositoiesContracts
{
 public interface IDayExerciseRepository : IRepository<ProgramDayExercise>
 {
 Task<IReadOnlyList<ProgramDayExercise>> GetByDayIdAsync(int dayId);
 }
}
