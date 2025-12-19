using ITI.Gymunity.FP.Application.DTOs.Trainer;
namespace ITI.Gymunity.FP.Application.Services
{
 public interface IReviewTrainerService
 {
 Task<TrainerReviewResponse> CreateAsync(string clientUserId, int trainerId, TrainerReviewCreateRequest request);
 Task<IEnumerable<TrainerAreaReviewResponse>> GetApprovedForTrainerAsync(int trainerId);
 }
}
