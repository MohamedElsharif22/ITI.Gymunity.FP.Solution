
using System.ComponentModel.DataAnnotations;

namespace ITI.Gymunity.FP.Application.DTOs.Program
{
    // Create Exercise Request
    public class CreateExerciseRequest
    {
        [Required]
        public string TrainerId { get; set; } = null!;

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Category { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string MuscleGroup { get; set; } = null!;

        [StringLength(100)]
        public string? Equipment { get; set; }

        [Url]
        public string? VideoDemoUrl { get; set; }

        [Url]
        public string? ThumbnailUrl { get; set; }

        [Required]
        [StringLength(200)]
        public string TrainerName { get; set; } = null!;
    }

}
