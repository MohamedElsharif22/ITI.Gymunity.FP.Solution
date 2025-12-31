using ITI.Gymunity.FP.Domain.Models.Enums;
using System;

namespace ITI.Gymunity.FP.Application.DTOs.Trainer
{
    public class SubscriberResponse
    {
        public string ClientId { get; set; } = null!;
        public string ClientName { get; set; } = string.Empty;
        public string ClientEmail { get; set; } = string.Empty;
        public string PackageName { get; set; } = string.Empty;
        public DateTime SubscriptionStartDate { get; set; }
        public DateTime SubscriptionEndDate { get; set; }
        public SubscriptionStatus Status { get; set; }
    }
}
