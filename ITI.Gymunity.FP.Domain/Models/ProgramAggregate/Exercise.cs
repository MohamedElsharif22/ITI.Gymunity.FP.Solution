using ITI.Gymunity.FP.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Domain.Models.ProgramAggregate
{
    // <summary>
    /// Global + custom exercises (custom when created by trainer)
    /// </summary>
    public class Exercise : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!; // Strength, Cardio, etc.
        public string MuscleGroup { get; set; } = null!;
        public string? Equipment { get; set; }
        public string? VideoDemoUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public bool IsCustom { get; set; } = false;
        public string? TrainerId { get; set; } // null = global library
        public AppUser? Trainer { get; set; }
    }
}
