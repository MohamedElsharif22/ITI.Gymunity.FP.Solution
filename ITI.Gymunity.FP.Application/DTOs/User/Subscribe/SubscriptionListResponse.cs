using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.DTOs.User.Subscribe
{
    public class SubscriptionListResponse
    {
        public int TotalSubscriptions { get; set; }
        public int ActiveSubscriptions { get; set; }
        public List<SubscriptionResponse> Subscriptions { get; set; } = new();
    }
}
