using ITI.Gymunity.FP.Domain.Models.Trainer;

namespace ITI.Gymunity.FP.Domain.RepositoiesContracts
{
    public interface IGuestReviewRepository : IRepository<TrainerReview>
    {
        Task<IReadOnlyList<TrainerReview>> GetApprovedByTrainerIdAsync(int trainerId);
        Task<IReadOnlyList<TrainerProfile>> GetTopTrainersByClientsAsync(int top);
    }
}
