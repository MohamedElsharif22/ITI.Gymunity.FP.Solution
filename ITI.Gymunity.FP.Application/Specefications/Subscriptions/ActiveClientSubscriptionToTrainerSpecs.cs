using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Specification;

namespace ITI.Gymunity.FP.Application.Specefications.Subscriptions
{
    public class ActiveClientSubscriptionToTrainerSpecs : BaseSpecification<Domain.Models.Subscription>
    {
        public ActiveClientSubscriptionToTrainerSpecs(string clientId, string trainerId)
            : base(s => s.ClientId == clientId
                     && s.Package.TrainerId == trainerId
                     && s.Status == SubscriptionStatus.Active
                     && s.CurrentPeriodEnd > DateTime.UtcNow)
        {
            AddInclude(s => s.Package);
        }
    }
}