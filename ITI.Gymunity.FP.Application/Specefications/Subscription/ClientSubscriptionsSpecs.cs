using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Specefications.Subscription
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
