using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Models.Identity;
using ITI.Gymunity.FP.Domain.Models.Trainer;

namespace ITI.Gymunity.FP.Domain.Models.ProgramAggregate
{
    public class Program : BaseEntity
    {
        public string TrainerId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public ProgramType Type { get; set; } // Workout, Nutrition, Hybrid, Challenge
        public int DurationWeeks { get; set; }
        public decimal? Price { get; set; } // null = only via subscription
        public bool IsPublic { get; set; } = true; // ⭐ New
        public bool IsActive { get; set; } = false; // ⭐ New
        public string? ImageUrl { get; set; } // ⭐ New (renamed from ThumbnailUrl)
        public int? MaxClients { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public AppUser Trainer { get; set; } = null!;
        public ICollection<ProgramWeek> Weeks { get; set; } = [];
        public ICollection<PackageProgram> PackagePrograms { get; set; } = [];
    }
}