using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Specification;

namespace ITI.Gymunity.FP.Application.Specefications
{
    public class ClientSubscriptionByIdSpecs : BaseSpecification<Subscription>
    {
        public ClientSubscriptionByIdSpecs(int id, string clientId)
            : base(s => s.Id == id && s.ClientId == clientId)
        {
            AddInclude(s => s.Package);
        }
    }
}