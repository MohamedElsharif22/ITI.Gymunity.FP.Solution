using ITI.Gymunity.FP.Domain.Models.Enums;
using System;
using System.Text.Json.Serialization;

namespace ITI.Gymunity.FP.Application.DTOs.Program
{
 public class ProgramCreateRequest
 {
 [JsonPropertyName("trainerUserId")]
 public string TrainerUserId { get; set; } = null!;

 // Accept legacy JSON field name 'trainerId' for backward compatibility
 [JsonPropertyName("trainerId")]
 public string TrainerId
 {
 // write-only alias: when incoming JSON has 'trainerId' it will set TrainerUserId
 set => TrainerUserId = value;
 }

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
