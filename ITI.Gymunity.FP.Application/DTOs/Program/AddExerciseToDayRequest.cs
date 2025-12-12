
using System.ComponentModel.DataAnnotations;

namespace ITI.Gymunity.FP.Application.DTOs.Program
{
    // Add Exercise to Day Request
    public class AddExerciseToDayRequest
    {
        [Required]
        public int ProgramDayId { get; set; }

        [Required]
        public int ExerciseId { get; set; }

        [Required]
        public int OrderIndex { get; set; }

        public string? Sets { get; set; }
        public string? Reps { get; set; }
        public int? RestSeconds { get; set; }
        public string? Tempo { get; set; }
        public decimal? RPE { get; set; }
        public decimal? Percent1RM { get; set; }
        public string? Notes { get; set; }
        public string? VideoUrl { get; set; }
        public string? ExerciseDataJson { get; set; }
    }
}