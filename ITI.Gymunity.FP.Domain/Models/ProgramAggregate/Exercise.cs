using ITI.Gymunity.FP.Domain.Models.Identity;

namespace ITI.Gymunity.FP.Domain.Models.ProgramAggregate
{
    /// <summary>
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
        public bool IsApproved { get; set; } = false; // ⭐ New
        public string? TrainerId { get; set; } // null = global library
        public string? TrainerName { get; set; } // ⭐ New

        public AppUser? Trainer { get; set; }
    }
}