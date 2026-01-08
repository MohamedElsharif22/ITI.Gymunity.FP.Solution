namespace ITI.Gymunity.FP.Application.DTOs.Trainer
{
 public class PackageWithProfitResponse
 {
 public int PackageId { get; set; }
 public string Name { get; set; } = null!;
 public string? Description { get; set; }
 public decimal PriceMonthly { get; set; }
 public decimal? PriceYearly { get; set; }
 public string Currency { get; set; } = "USD";
 public int? TrainerProfileId { get; set; }
 public string? TrainerUserId { get; set; }
 public string? TrainerName { get; set; }
 public bool IsActive { get; set; }
 public DateTime CreatedAt { get; set; }
 public decimal Profit { get; set; }
 }
}
