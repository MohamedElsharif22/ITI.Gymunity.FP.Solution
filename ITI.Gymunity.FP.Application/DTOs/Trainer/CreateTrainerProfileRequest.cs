namespace ITI.Gymunity.FP.Application.DTOs.Trainer
{
    public class CreateTrainerProfileRequest
    {
        public string UserId { get; set; } = null!;
        public string Handle { get; set; } = null!;
        public string Bio { get; set; } = string.Empty;
        public string? CoverImageUrl { get; set; }
        public string? VideoIntroUrl { get; set; }
        public string? BrandingColors { get; set; }
        public int YearsExperience { get; set; }
    }
}
