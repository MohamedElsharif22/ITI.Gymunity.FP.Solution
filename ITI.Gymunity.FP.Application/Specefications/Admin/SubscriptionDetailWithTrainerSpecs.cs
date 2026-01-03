using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Specification;
using Microsoft.EntityFrameworkCore;

namespace ITI.Gymunity.FP.Application.Specefications.Admin
{
    /// <summary>
    /// Specification for retrieving detailed subscription information for admin management
    /// Includes client with user data, package with trainer, and payment information
    /// Optimized for admin details view
    /// </summary>
    public class SubscriptionDetailWithTrainerSpecs : BaseSpecification<Subscription>
    {
        public SubscriptionDetailWithTrainerSpecs(int subscriptionId)
        {
            // Filter by specific subscription ID
            Criteria = s => s.Id == subscriptionId && !s.IsDeleted;

            // Eager load client (AppUser directly, not a User property)
            AddInclude(s => s.Client);

            // Eager load package with trainer and user data
            AddInclude(q => q.Include(s => s.Package)
                .ThenInclude(p => p.Trainer!.User));

            // Eager load payments for this subscription
            AddInclude(s => s.Payments);

            // Order by creation date
            AddOrderByDesc(s => s.StartDate);
        }
    }
}
