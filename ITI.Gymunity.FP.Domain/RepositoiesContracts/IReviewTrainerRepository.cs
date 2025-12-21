using ITI.Gymunity.FP.Domain.Models.Trainer;

namespace ITI.Gymunity.FP.Domain.RepositoiesContracts
{
 public interface IReviewTrainerRepository : IRepository<TrainerReview>
 {
 Task<IReadOnlyList<TrainerReview>> GetByTrainerIdAsync(int trainerId);
 }
}
