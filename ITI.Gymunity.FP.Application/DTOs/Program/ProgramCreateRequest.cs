using ITI.Gymunity.FP.Application.Validation;
using ITI.Gymunity.FP.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ITI.Gymunity.FP.Application.DTOs.Program
{
    public class ProgramCreateRequest
    {
        [JsonPropertyName("trainerProfileId")]
        public int TrainerProfileId { get; set; }

        // Accept legacy trainerId (int)
        [JsonPropertyName("trainerId")]
        public int TrainerIdLegacy { set => TrainerProfileId = value; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Title must be between3 and200 characters.")]
        [UniqueProgramTitle]
        public string Title { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public ProgramType Type { get; set; }
        public int DurationWeeks { get; set; }
        public decimal? Price { get; set; }
        public bool IsPublic { get; set; } = true;
        public int? MaxClients { get; set; }
        public string? ThumbnailUrl { get; set; }
    }
}
