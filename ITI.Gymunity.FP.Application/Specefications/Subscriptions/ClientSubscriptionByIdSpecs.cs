using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Specification;
using Microsoft.EntityFrameworkCore;

namespace ITI.Gymunity.FP.Application.Specefications.Subscriptions
{
    /// <summary>
    /// Get single subscription by ID for a specific client
    /// </summary>
    public class ClientSubscriptionByIdSpecs : BaseSpecification<Subscription>
    {
        public ClientSubscriptionByIdSpecs(int id, string clientId)
        : base(s => s.Id == id && s.ClientId == clientId)
        {
            // ✅ لازم يبقى موجود!
            AddInclude(query => query
                .Include(s => s.Package)
                    .ThenInclude(p => p.Trainer)
                        .ThenInclude(t => t.User));
        }
    }
}