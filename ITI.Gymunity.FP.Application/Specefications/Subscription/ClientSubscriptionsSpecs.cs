using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Specification;

namespace ITI.Gymunity.FP.Infrastructure.Specefications.Subscription
{
    public class ClientSubscriptionsSpecs : BaseSpecification<Domain.Models.Subscription>
    {
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