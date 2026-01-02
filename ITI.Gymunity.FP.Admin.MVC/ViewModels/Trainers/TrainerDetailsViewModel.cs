using System;

namespace ITI.Gymunity.FP.Admin.MVC.ViewModels.Trainers
{
    public class TrainerDetailsViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Handle { get; set; } = null!;
        public string Bio { get; set; } = string.Empty;
        public string? CoverImageUrl { get; set; }
        public string? VideoIntroUrl { get; set; }
        public string? BrandingColors { get; set; }
        public bool IsVerified { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public bool IsSuspended { get; set; }
        public DateTime? SuspendedAt { get; set; }
        public decimal RatingAverage { get; set; }
        public int TotalClients { get; set; }
        public int YearsExperience { get; set; }
        public string? StatusImageUrl { get; set; }
        public string? StatusDescription { get; set; }
        public decimal AvailableBalance { get; set; } = 0m;
        public DateTimeOffset CreatedAt { get; set; }
    }
}
