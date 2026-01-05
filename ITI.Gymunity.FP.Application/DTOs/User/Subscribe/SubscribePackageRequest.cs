using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITI.Gymunity.FP.Domain.Models.Enums;

namespace ITI.Gymunity.FP.Application.DTOs.User.Subscribe
{
    public class SubscribePackageRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Package ID")]
        public int PackageId { get; set; }

        public bool IsAnnual { get; set; } = false;

        public string? PromoCode { get; set; }

        // ✅ NEW: Payment method support
        public PaymentMethod? PaymentMethod { get; set; }

        // ✅ NEW: Return URL for payment confirmation
        public string? ReturnUrl { get; set; }
    }
}
