using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.DTOs.Program
{
    // Exercise Response
    public class ExerciseResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string MuscleGroup { get; set; } = null!;
        public string? Equipment { get; set; }
        public string? VideoDemoUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public bool IsCustom { get; set; }
        public bool IsApproved { get; set; }
        public string? TrainerId { get; set; }
        public string? TrainerName { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
