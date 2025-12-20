using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Specification;

namespace ITI.Gymunity.FP.Application.Specefications.Subscription
{
    public class ActiveClientSubscriptionForPackageSpecs : BaseSpecification<Domain.Models.Subscription>
    {
        public ActiveClientSubscriptionForPackageSpecs(string clientId, int packageId)
            : base(s => s.ClientId == clientId
                     && s.PackageId == packageId
                     && (s.Status == SubscriptionStatus.Active
                         || s.Status == SubscriptionStatus.Unpaid))
        {
            AddInclude(s => s.Package);
        }
    }
}