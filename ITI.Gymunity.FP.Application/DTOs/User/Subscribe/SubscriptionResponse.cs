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
        public string PackageName { get; set; } = null!;
        public decimal AmountPaid { get; set; }
        public SubscriptionStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CurrentPeriodEnd { get; set; }
    }
}
