namespace ITI.Gymunity.FP.Application.DTOs.Admin
{
 public class AdminReviewActionResponse
 {
 public int Id { get; set; }
 public bool IsApproved { get; set; }
 public DateTimeOffset? ApprovedAt { get; set; }
 public string Message { get; set; } = string.Empty;
 }
}
