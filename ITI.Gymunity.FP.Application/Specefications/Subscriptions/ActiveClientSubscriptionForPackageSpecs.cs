using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Specification;
using Microsoft.EntityFrameworkCore;

namespace ITI.Gymunity.FP.Application.Specefications.Subscriptions
{
    /// <summary>
    /// Check if client already has active/unpaid subscription for specific package
    /// (Used to prevent duplicate subscriptions)
    /// </summary>
    public class ActiveClientSubscriptionForPackageSpecs : BaseSpecification<Subscription>
    {
        public ActiveClientSubscriptionForPackageSpecs(string clientId, int packageId)
            : base(s => s.ClientId == clientId
                     && s.PackageId == packageId
                     && (s.Status == SubscriptionStatus.Active
                         || s.Status == SubscriptionStatus.Unpaid))
        {
            AddInclude(query => query.Include(s => s.Package));
        }
    }
}