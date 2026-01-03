using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Specification;
using Microsoft.EntityFrameworkCore;

namespace ITI.Gymunity.FP.Application.Specefications.Admin
{
    /// <summary>
    /// Specification for retrieving active subscriptions with payments for revenue calculation
    /// Optimized for statistics and reporting
    /// </summary>
    public class ActiveSubscriptionsWithPaymentsSpecs : BaseSpecification<Subscription>
    {
        public ActiveSubscriptionsWithPaymentsSpecs()
        {
            // Filter for active subscriptions only
            Criteria = s => s.Status == SubscriptionStatus.Active && !s.IsDeleted;

            // Eager load payments for revenue calculation
            AddInclude(s => s.Payments);

            // Order by most recent
            AddOrderByDesc(s => s.StartDate);
        }
    }
}
