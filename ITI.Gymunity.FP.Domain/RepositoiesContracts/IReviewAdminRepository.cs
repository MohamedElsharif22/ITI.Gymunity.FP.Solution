using ITI.Gymunity.FP.Domain.Models.Trainer;

namespace ITI.Gymunity.FP.Domain.RepositoiesContracts
{
 public interface IReviewAdminRepository : IRepository<TrainerReview>
 {
 Task<IReadOnlyList<TrainerReview>> GetAllPendingAsync();
 }
}
