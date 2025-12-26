using ITI.Gymunity.FP.Domain.Models.Enums;
using System;
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
