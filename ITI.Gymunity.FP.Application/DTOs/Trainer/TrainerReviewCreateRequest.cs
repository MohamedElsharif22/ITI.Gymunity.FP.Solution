using System.ComponentModel.DataAnnotations;

namespace ITI.Gymunity.FP.Application.DTOs.Trainer
{
 public class TrainerReviewCreateRequest
 {
 public string ClientId { get; set; } = null!; //edited for prevent authirezed state 
 [Required]
 [Range(1,5, ErrorMessage = "Rating must be between1 and5")]
 public int Rating { get; set; }

 [StringLength(1000, ErrorMessage = "Comment must not exceed1000 characters")]
 public string? Comment { get; set; }
 }
}
