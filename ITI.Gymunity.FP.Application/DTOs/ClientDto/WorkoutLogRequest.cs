using ITI.Gymunity.FP.Domain.Models.Client;
using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.DTOs.ClientDto
{
    public class WorkoutLogRequest
    {
        [Required(ErrorMessage = "ProgramDayId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid ProgramDayId")]
        public int ProgramDayId { get; set; }

        [Required(ErrorMessage = "CompletedAt date is required")]
        public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(2000, ErrorMessage = "Notes cannot exceed 2000 characters")]
        public string? Notes { get; set; }

        [Range(1, 600, ErrorMessage = "Duration must be between 1 and 600 minutes")]
        public int? DurationMinutes { get; set; }

        [Required(ErrorMessage = "Exercise log data is required")]
        [MaxLength(10000, ErrorMessage = "Exercise log data is too large")]
        [MinLength(2, ErrorMessage = "Exercise log must contain valid JSON")]
        public string ExercisesLoggedJson { get; set; } = "[]";
    }
}
