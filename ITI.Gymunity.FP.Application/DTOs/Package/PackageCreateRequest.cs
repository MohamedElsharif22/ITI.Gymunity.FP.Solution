using System.ComponentModel.DataAnnotations;

namespace ITI.Gymunity.FP.Application.DTOs.Trainer
{
    public class PackageCreateRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = null!;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 100000)]
        public decimal PriceMonthly { get; set; }

        [Range(0.01, 100000)]
        public decimal? PriceYearly { get; set; }

        public bool IsActive { get; set; } = true;
        public string? ThumbnailUrl { get; set; }
        public int[] ProgramIds { get; set; } = new int[0];

        // New
        public bool IsAnnual { get; set; }
        [StringLength(20, MinimumLength = 3)]
        public string? PromoCode { get; set; }

        // TrainerId added so controller can accept trainer id in request for dev/testing
        [Required(ErrorMessage = "TrainerId is required")]
        [StringLength(450, ErrorMessage = "TrainerId length must be <=450 characters")]
        public string? TrainerId { get; set; }
    }
}
