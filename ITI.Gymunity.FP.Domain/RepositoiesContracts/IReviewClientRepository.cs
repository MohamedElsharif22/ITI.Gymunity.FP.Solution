using ITI.Gymunity.FP.Domain.Models.Trainer;

namespace ITI.Gymunity.FP.Domain.RepositoiesContracts
{
 public interface IReviewClientRepository : IRepository<TrainerReview>
 {
 Task<IReadOnlyList<TrainerReview>> GetByTrainerIdAsync(int trainerId);
 }
}
