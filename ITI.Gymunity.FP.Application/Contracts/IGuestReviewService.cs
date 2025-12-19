using ITI.Gymunity.FP.Application.DTOs.Guest;

namespace ITI.Gymunity.FP.Application.Services
{
 public interface IGuestReviewService
 {
 Task<GuestReviewsByTrainerResponse> GetApprovedReviewsByTrainerAsync(int trainerProfileId);
 Task<TopTrainersResponse> GetTopTrainersAsync();
 }
}
