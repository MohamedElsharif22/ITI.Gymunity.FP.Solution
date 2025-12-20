using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Specification;

namespace ITI.Gymunity.FP.Application.Specefications.Subscription
{
    public class ClientSubscriptionByIdSpecs : BaseSpecification<Domain.Models.Subscription>
    {
        public ClientSubscriptionByIdSpecs(int id, string clientId)
            : base(s => s.Id == id && s.ClientId == clientId)
        {
            AddInclude(s => s.Package);
            AddInclude(s => s.Package.Trainer);        
            AddInclude(s => s.Package.Trainer);
        }
    }
}