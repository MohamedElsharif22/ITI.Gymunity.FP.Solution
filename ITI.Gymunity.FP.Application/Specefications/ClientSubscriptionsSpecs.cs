using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Specification;

public class ClientSubscriptionsSpecs : BaseSpecification<Subscription>
{
    public ClientSubscriptionsSpecs(string clientId)
        : base(s => s.ClientId == clientId)
    {
        AddInclude(s => s.Package);
        AddOrderByDesc(s => s.CreatedAt);
    }
}
