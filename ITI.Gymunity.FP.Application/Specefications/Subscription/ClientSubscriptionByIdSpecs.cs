using ITI.Gymunity.FP.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Specefications.Subscription
{
    public class ClientSubscriptionByIdSpecs : BaseSpecification<Domain.Models.Subscription>
    {
        public ClientSubscriptionByIdSpecs(int id, string clientId)
            : base(s => s.Id == id && s.ClientId == clientId)
        {
            AddInclude(s => s.Package);
        }
    }
}
