using ITI.Gymunity.FP.Application.DTOs.Admin;
using ITI.Gymunity.FP.Application.DTOs.Trainer;
namespace ITI.Gymunity.FP.Application.Services
{
 public interface IReviewAdminService
 {
 Task<AdminReviewActionResponse?> ApproveAsync(int reviewId);
 Task<AdminReviewActionResponse?> RejectAsync(int reviewId);
 Task<bool> DeletePermanentAsync(int reviewId);
 Task<IReadOnlyList<AdminReviewActionResponse>> GetPendingAsync();
 Task<IReadOnlyList<TrainerReviewResponse>> GetAllPendingAsync();
 }
}
