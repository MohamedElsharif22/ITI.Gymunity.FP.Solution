namespace ITI.Gymunity.FP.Application.DTOs.Trainer
{
 public class TrainerAreaReviewResponse
 {
 public int Id { get; set; }
 public int ClientId { get; set; }
 public int Rating { get; set; }
 public string? Comment { get; set; }
 public DateTimeOffset CreatedAt { get; set; }
 }
}
