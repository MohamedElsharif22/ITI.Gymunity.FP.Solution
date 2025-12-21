using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using ITI.Gymunity.FP.Infrastructure._Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Infrastructure.Repositories
{
 public class WeekRepository : Repository<ProgramWeek>, IWeekRepository
 {
 public WeekRepository(AppDbContext context) : base(context) { }

 public async Task<IReadOnlyList<ProgramWeek>> GetByProgramIdAsync(int programId)
 {
 return await _Context.ProgramWeeks.Where(w => w.ProgramId == programId).ToListAsync();
 }

 public async Task<ProgramWeek?> GetWithDaysAsync(int id)
 {
 return await _Context.ProgramWeeks.Include(w => w.Days).FirstOrDefaultAsync(w => w.Id == id);
 }
 }
}
