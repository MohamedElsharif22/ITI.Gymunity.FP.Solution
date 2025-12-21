using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using ITI.Gymunity.FP.Infrastructure._Data;
using Microsoft.EntityFrameworkCore;

namespace ITI.Gymunity.FP.Infrastructure.Repositories
{
 internal class GuestReviewRepository(AppDbContext dbContext) : Repository<TrainerReview>(dbContext), IGuestReviewRepository
 {
 public async Task<IReadOnlyList<TrainerReview>> GetApprovedByTrainerIdAsync(int trainerId)
 {
 return await _Context.Set<TrainerReview>()
 .Where(r => r.TrainerId == trainerId && r.IsApproved)
 .Include(r => r.Client).ThenInclude(c => c.User)
 .ToListAsync();
 }

 public async Task<IReadOnlyList<TrainerProfile>> GetTopTrainersByClientsAsync(int top)
 {
 return await _Context.Set<TrainerProfile>()
 .OrderByDescending(tp => tp.TotalClients)
 .Take(top)
 .ToListAsync();
 }
 }
}
