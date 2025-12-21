using ITI.Gymunity.FP.Domain.Models.Enums;
using System;

namespace ITI.Gymunity.FP.Application.DTOs.Client
{
 public class ProgramClientResponse
 {
 public int Id { get; set; }
 public string Title { get; set; } = null!;
 public string Description { get; set; } = string.Empty;
 public ProgramType Type { get; set; }
 public int DurationWeeks { get; set; }
 public decimal? Price { get; set; }
 public bool IsPublic { get; set; }
 public int? MaxClients { get; set; }
 public string? ThumbnailUrl { get; set; }
 public DateTime CreatedAt { get; set; }
 public DateTime UpdatedAt { get; set; }

 // Trainer info
 public string TrainerId { get; set; } = null!;
 public string? TrainerUserName { get; set; }
 public string? TrainerHandle { get; set; }
 }
}
