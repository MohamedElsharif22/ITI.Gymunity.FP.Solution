using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Domain.RepositoiesContracts
{
 public interface IWeekRepository : IRepository<ProgramWeek>
 {
 Task<IReadOnlyList<ProgramWeek>> GetByProgramIdAsync(int programId);
 Task<ProgramWeek?> GetWithDaysAsync(int id);
 }
}
