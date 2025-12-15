namespace ITI.Gymunity.FP.Application.DTOs.Trainer
{
 public class PackageCreateRequest
 {
 public string Name { get; set; } = null!;
 public string Description { get; set; } = string.Empty;
 public decimal PriceMonthly { get; set; }
 public decimal? PriceYearly { get; set; }
 public bool IsActive { get; set; } = true;
 public string? ThumbnailUrl { get; set; }
 public int[] ProgramIds { get; set; } = new int[0];
 }
}
