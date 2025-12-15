using System;

namespace ITI.Gymunity.FP.Application.DTOs.Client
{
 public class PackageClientResponse
 {
 public int Id { get; set; }
 public string Name { get; set; } = null!;
 public string Description { get; set; } = string.Empty;
 public decimal PriceMonthly { get; set; }
 public decimal? PriceYearly { get; set; }
 public bool IsActive { get; set; }
 public string? ThumbnailUrl { get; set; }
 public string TrainerId { get; set; } = null!;
 }
}
