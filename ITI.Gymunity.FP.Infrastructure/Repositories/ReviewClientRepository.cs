using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using ITI.Gymunity.FP.Infrastructure._Data;
using Microsoft.EntityFrameworkCore;

namespace ITI.Gymunity.FP.Infrastructure.Repositories
{
 internal class ReviewClientRepository(AppDbContext dbContext) : Repository<TrainerReview>(dbContext), IReviewClientRepository
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
