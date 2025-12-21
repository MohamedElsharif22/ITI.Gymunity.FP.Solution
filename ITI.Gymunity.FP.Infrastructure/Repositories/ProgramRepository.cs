using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using ITI.Gymunity.FP.Infrastructure._Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Infrastructure.Repositories
{
 public class ProgramRepository : Repository<Program>, IProgramRepository
 {
 public ProgramRepository(AppDbContext context) : base(context)
 {
 }

 public async Task<IReadOnlyList<Program>> GetByTrainerAsync(string trainerId)
 {
 return await _Context.Programs.Where(p => p.TrainerId == trainerId).ToListAsync();
 }

 public async Task<Program?> GetByIdWithIncludesAsync(int id)
 {
 return await _Context.Programs.Include(p => p.Weeks).ThenInclude(w => w.Days).ThenInclude(d => d.Exercises)
 .FirstOrDefaultAsync(p => p.Id == id);
 }

 public async Task<IReadOnlyList<Program>> SearchAsync(string? term)
 {
 var query = _Context.Programs.AsQueryable();
 if (!string.IsNullOrWhiteSpace(term))
 query = query.Where(p => p.Title.Contains(term) || p.Description.Contains(term));
 return await query.ToListAsync();
 }
 }
}
