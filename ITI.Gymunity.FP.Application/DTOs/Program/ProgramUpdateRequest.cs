using ITI.Gymunity.FP.Domain.Models.Enums;
using System;

namespace ITI.Gymunity.FP.Application.DTOs.Program
{
 public class ProgramUpdateRequest
 {
 public string Title { get; set; } = null!;
 public string Description { get; set; } = string.Empty;
 public ProgramType Type { get; set; }
 public int DurationWeeks { get; set; }
 public decimal? Price { get; set; }
 public bool IsPublic { get; set; }
 public int? MaxClients { get; set; }
 public string? ThumbnailUrl { get; set; }
 }
}
