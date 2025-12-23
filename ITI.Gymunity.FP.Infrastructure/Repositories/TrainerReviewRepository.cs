using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using Microsoft.EntityFrameworkCore;
using ITI.Gymunity.FP.Infrastructure._Data;

namespace ITI.Gymunity.FP.Infrastructure.Repositories
{
 internal class TrainerReviewRepository(AppDbContext dbContext) : Repository<TrainerReview>(dbContext), ITrainerReviewRepository
 {
 public async Task<IReadOnlyList<TrainerReview>> GetByTrainerIdAsync(int trainerId)
 {
 return await _Context.Set<TrainerReview>()
 .Where(r => r.TrainerId == trainerId)
 .Include(r => r.Client)
 .ToListAsync();
 }
 }
}
