using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Specification;
using Microsoft.EntityFrameworkCore;

namespace ITI.Gymunity.FP.Application.Specefications.Subscriptions
{
    /// <summary>
    /// Get all client subscriptions with optional status filter
    /// </summary>
    public class ClientSubscriptionsSpecs : BaseSpecification<Subscription>
    {
        public ClientSubscriptionsSpecs(string clientId, SubscriptionStatus? status = null)
            : base(s => s.ClientId == clientId
                     && (!status.HasValue || s.Status == status.Value))
        {
            // Include Package → Trainer → User
            AddInclude(query => query
                .Include(s => s.Package)
                    .ThenInclude(p => p.Trainer)
                        .ThenInclude(t => t.User));

            AddOrderByDesc(s => s.CreatedAt);
        }
    }
}