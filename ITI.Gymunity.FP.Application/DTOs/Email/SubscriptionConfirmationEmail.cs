using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Infrastructure.DTOs.Email
{
    public class SubscriptionConfirmationEmail
    {
        public string ClientName { get; set; } = null!;
        public string ClientEmail { get; set; } = null!;
        public string PackageName { get; set; } = null!;
        public string TrainerName { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
        public DateTime SubscriptionStartDate { get; set; }
        public DateTime SubscriptionEndDate { get; set; }
    }
}
