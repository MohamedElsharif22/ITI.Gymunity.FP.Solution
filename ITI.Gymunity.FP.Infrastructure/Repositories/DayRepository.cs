using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using ITI.Gymunity.FP.Infrastructure._Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Infrastructure.Repositories
{
 public class DayRepository : Repository<ProgramDay>, IDayRepository
 {
 public DayRepository(AppDbContext context) : base(context) { }

 public async Task<IReadOnlyList<ProgramDay>> GetByWeekIdAsync(int weekId)
 {
 return await _Context.ProgramDays.Where(d => d.ProgramWeekId == weekId).ToListAsync();
 }

 public async Task<ProgramDay?> GetWithExercisesAsync(int id)
 {
 return await _Context.ProgramDays.Include(d => d.Exercises).FirstOrDefaultAsync(d => d.Id == id);
 }
 }
}
