using ITI.Gymunity.FP.Application.DTOs.Trainer;
namespace ITI.Gymunity.FP.Application.Services
{
 public interface IReviewClientService
 {
 Task<TrainerReviewResponse> CreateAsync(string clientUserId, int trainerId, TrainerReviewCreateRequest request);
 Task<TrainerReviewResponse?> UpdateAsync(string clientUserId, int reviewId, TrainerReviewCreateRequest request);
 Task<bool> DeleteAsync(string clientUserId, int reviewId);
 }
}
