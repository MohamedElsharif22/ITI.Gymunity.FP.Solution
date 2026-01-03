using System.ComponentModel.DataAnnotations;

namespace ITI.Gymunity.FP.Application.DTOs.Program
{
 public class ProgramAiCreateRequest
 {
 [Required]
 public int TrainerProfileId { get; set; }

 [Required]
 public string Goal { get; set; } = null!; // e.g. "Muscle gain"

 [Range(1,520)]
 public int DurationWeeks { get; set; } =4;

 [Range(1,7)]
 public int DaysPerWeek { get; set; } =3;

 [Range(1,20)]
 public int ExercisesPerDay { get; set; } =5;

 // Optional additional details to help AI
 public string? Notes { get; set; }
 }
}