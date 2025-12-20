using ITI.Gymunity.FP.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.DTOs.User.Subscribe
{
    public class SubscriptionResponse
    {
        public int Id { get; set; }

        // Package Info
        public int PackageId { get; set; }
        public string PackageName { get; set; } = null!;
        public string PackageDescription { get; set; } = null!;

        // Trainer Info
        public string TrainerId { get; set; } = null!;
        public string TrainerName { get; set; } = null!;
        public string TrainerHandle { get; set; } = null!;
        public string? TrainerPhotoUrl { get; set; }

        // Subscription Details
        public SubscriptionStatus Status { get; set; }
        public decimal AmountPaid { get; set; }
        public string Currency { get; set; } = "EGP";
        public bool IsAnnual { get; set; }

        // Dates
        public DateTime StartDate { get; set; }
        public DateTime CurrentPeriodEnd { get; set; }
        public DateTime? CanceledAt { get; set; }

        // Computed Properties
        public int DaysRemaining => (CurrentPeriodEnd - DateTime.UtcNow).Days;
        public bool IsExpiringSoon => DaysRemaining <= 7 && DaysRemaining > 0;
        public bool HasExpired => DateTime.UtcNow > CurrentPeriodEnd;

        // Features
        public List<string> FeaturesIncluded { get; set; } = new();
    }
}
