using ITI.Gymunity.FP.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Domain.Models
{
    public class Payment : BaseEntity
    {
        public int SubscriptionId { get; set; }

        public string? PaymobTransactionId { get; set; }
        public string? PayPalPaymentId { get; set; }

        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EGP";
        public decimal PlatformFee { get; set; }
        public decimal TrainerPayout { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime PaidAt { get; set; }

        public Subscription Subscription { get; set; } = null!;
    }
}
