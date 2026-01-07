using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain.Specification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Specefications.Trainer
{
    public class TrainersByClientUserIdSpecification : BaseSpecification<TrainerProfile>
    {
        public TrainersByClientUserIdSpecification(string clientUserId)
        {
            // Filter trainers associated with the specified client user ID
            // This checks if the trainer has any subscriptions where the client ID matches
            Criteria = tp => tp.User.Subscriptions
                .Where(s => s.Client.Id == clientUserId && s.Status == SubscriptionStatus.Active)
                .Any();
            
            // Include User data and subscriptions with client details for mapping
            AddInclude(q =>
                q.Include(tp => tp.User)
                    .ThenInclude(u => u.Subscriptions)
                    .ThenInclude(s => s.Client));
        }
    }
}
