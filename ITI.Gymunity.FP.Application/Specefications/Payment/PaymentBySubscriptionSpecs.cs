using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Specification;

namespace ITI.Gymunity.FP.Application.Specefications.Payment
{
    public class PaymentBySubscriptionSpecs : BaseSpecification<Domain.Models.Payment>
    {
        public PaymentBySubscriptionSpecs(int subscriptionId)
             : base(p =>
                p.SubscriptionId == subscriptionId &&
                p.Status == PaymentStatus.Pending &&
                !p.IsDeleted)
        {
            AddInclude(p => p.Subscription);
            AddOrderByDesc(p => p.CreatedAt);
        }

        public PaymentBySubscriptionSpecs(int subscriptionId, string clientId)
             : base(p =>
                p.SubscriptionId == subscriptionId &&
                p.ClientId == clientId &&
                !p.IsDeleted)
        {
            AddInclude(p => p.Subscription);
            AddOrderByDesc(p => p.CreatedAt);
        }
    }
}
    