using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Models.Identity;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Domain.Models
{
    public class Subscription : BaseEntity
    {
        public string ClientId { get; set; } = null!;
        public int PackageId { get; set; }

        public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime CurrentPeriodEnd { get; set; }

        // Paymob
        public string? PaymobOrderId { get; set; }
        public string? PaymobTransactionId { get; set; }

        // PayPal
        public string? PayPalSubscriptionId { get; set; }

        public string Currency { get; set; } = "EGP";
        public decimal AmountPaid { get; set; }
        public decimal PlatformFeePercentage { get; set; } = 15m;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CanceledAt { get; set; }

        public AppUser Client { get; set; } = null!;
        public Package Package { get; set; } = null!;
        public ICollection<Payment> Payments { get; set; } = [];
    }
}
