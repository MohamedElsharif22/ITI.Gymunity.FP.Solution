using ITI.Gymunity.FP.Domain.Models.Enums;

namespace ITI.Gymunity.FP.Application.DTOs.User.Payment
{
    public class PaymentResponse
    {
        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public PaymentStatus Status { get; set; }
        public PaymentMethod Method { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public string? FailureReason { get; set; }

        // Payment Gateway Data
        public string? PaymentUrl { get; set; }
        public string? TransactionId { get; set; }
    }
}