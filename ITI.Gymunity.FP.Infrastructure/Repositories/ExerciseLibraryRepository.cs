using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using ITI.Gymunity.FP.Infrastructure._Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Infrastructure.Repositories
{
 public class ExerciseLibraryRepository : Repository<Exercise>, IExerciseLibraryRepository
 {
 public ExerciseLibraryRepository(AppDbContext context) : base(context)
 {
 }

 public async Task<IReadOnlyList<Exercise>> SearchByNameAsync(string? name, string? trainerId = null)
 {
 var query = _Context.Set<Exercise>().AsQueryable();
 if (!string.IsNullOrWhiteSpace(name))
 query = query.Where(e => e.Name.Contains(name));
 if (!string.IsNullOrWhiteSpace(trainerId))
 query = query.Where(e => e.TrainerId == trainerId);
 return await query.ToListAsync();
 }
 }
}
