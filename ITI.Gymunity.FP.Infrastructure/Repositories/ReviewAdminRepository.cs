using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using Microsoft.EntityFrameworkCore;
using ITI.Gymunity.FP.Infrastructure._Data;

namespace ITI.Gymunity.FP.Infrastructure.Repositories
{
 internal class ReviewAdminRepository(AppDbContext dbContext) : Repository<TrainerReview>(dbContext), IReviewAdminRepository
 {
 public async Task<IReadOnlyList<TrainerReview>> GetAllPendingAsync()
 {
 return await _Context.Set<TrainerReview>()
 .Where(r => !r.IsApproved)
 .Include(r => r.Client)
 .Include(r => r.Trainer)
 .ToListAsync();
 }
 }
}
