using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Specification;
using Microsoft.EntityFrameworkCore;

namespace ITI.Gymunity.FP.Application.Specefications.Subscriptions
{
    /// <summary>
    /// Check if client has active subscription to a specific trainer
    /// </summary>
    public class ActiveClientSubscriptionToTrainerSpecs : BaseSpecification<Subscription>
    {
        public ActiveClientSubscriptionToTrainerSpecs(string clientId, int trainerId)
            : base(s => s.ClientId == clientId
                     && s.Package.TrainerId == trainerId
                     && s.Status == SubscriptionStatus.Active
                     && s.CurrentPeriodEnd > DateTime.UtcNow)
        {
            AddInclude(query => query
                .Include(s => s.Package)
                    .ThenInclude(p => p.Trainer));
        }
    }
}