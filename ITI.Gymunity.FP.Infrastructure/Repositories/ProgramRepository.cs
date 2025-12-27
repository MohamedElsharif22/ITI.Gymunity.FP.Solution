using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.Gymunity.FP.Infrastructure._Data;

namespace ITI.Gymunity.FP.Infrastructure.Repositories
{
 public class ProgramRepository : Repository<Program>, IProgramRepository
 {
 public ProgramRepository(AppDbContext context) : base(context)
 {
 }

 // legacy: still support trainerId as user id string by joining via TrainerProfile
 public async Task<IReadOnlyList<Program>> GetByTrainerAsync(string trainerUserId)
 {
 return await _Context.Programs.Where(p => p.TrainerProfileId != null && p.TrainerProfile.UserId == trainerUserId).ToListAsync();
 }

 public async Task<IReadOnlyList<Program>> GetByTrainerAsyncProfileId(int trainerProfileId)
 {
 return await _Context.Programs.Where(p => p.TrainerProfileId == trainerProfileId).ToListAsync();
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

 public async Task<bool> ExistsByTitleAsync(string title)
 {
 if (string.IsNullOrWhiteSpace(title)) return false;
 var normalized = title.Trim();
 return await _Context.Programs.AnyAsync(p => p.Title != null && p.Title.ToLower() == normalized.ToLower());
 }
 }
}
