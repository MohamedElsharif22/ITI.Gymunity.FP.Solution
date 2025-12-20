using System.ComponentModel.DataAnnotations;
using ITI.Gymunity.FP.Domain.Models.Enums;

namespace ITI.Gymunity.FP.Application.DTOs.User.Payment
{
    public class InitiatePaymentRequest
    {
        [Required]
        public int SubscriptionId { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        public string? ReturnUrl { get; set; }
    }
}