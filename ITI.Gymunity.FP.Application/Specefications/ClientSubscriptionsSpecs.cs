// Application/Specefications/Subscription/ClientSubscriptionsSpecs.cs

using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Specification;

namespace ITI.Gymunity.FP.Application.Specefications.Subscription
{
    public class ClientSubscriptionsSpecs : BaseSpecification<Domain.Models.Subscription>
    {
        // ✅ Constructor يقبل 2 parameters
        public ClientSubscriptionsSpecs(string clientId, SubscriptionStatus? status = null)
            : base(s => s.ClientId == clientId
                     && (!status.HasValue || s.Status == status.Value))
        {
            AddInclude(s => s.Package);
            AddInclude(s => s.Package.Trainer);
            AddOrderByDesc(s => s.CreatedAt);
        }
    }
}