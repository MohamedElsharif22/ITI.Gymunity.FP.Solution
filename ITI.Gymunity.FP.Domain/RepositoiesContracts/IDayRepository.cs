using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Domain.RepositoiesContracts
{
 public interface IDayRepository : IRepository<ProgramDay>
 {
 Task<IReadOnlyList<ProgramDay>> GetByWeekIdAsync(int weekId);
 Task<ProgramDay?> GetWithExercisesAsync(int id);
 }
}
