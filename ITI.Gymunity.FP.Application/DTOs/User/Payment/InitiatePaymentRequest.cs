using ITI.Gymunity.FP.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ITI.Gymunity.FP.Application.DTOs.User.Payment
{
    public class InitiatePaymentRequest
    {
        [Required]
        public int SubscriptionId { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethod PaymentMethod { get; set; }

        public string? ReturnUrl { get; set; }
    }
}