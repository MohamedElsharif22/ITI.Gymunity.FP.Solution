using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using ITI.Gymunity.FP.Infrastructure._Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Infrastructure.Repositories
{
 public class DayExerciseRepository : Repository<ProgramDayExercise>, IDayExerciseRepository
 {
 public DayExerciseRepository(AppDbContext context) : base(context) { }

 public async Task<IReadOnlyList<ProgramDayExercise>> GetByDayIdAsync(int dayId)
 {
 return await _Context.ProgramDayExercises.Where(e => e.ProgramDayId == dayId).ToListAsync();
 }
 }
}
