using ITI.Gymunity.FP.Application.DTOs.Trainer;
namespace ITI.Gymunity.FP.Application.Services
{
 public interface ITrainerReviewService
 {
 Task<TrainerReviewResponse> CreateAsync(string clientUserId, int trainerId, TrainerReviewCreateRequest request);
 }
}
