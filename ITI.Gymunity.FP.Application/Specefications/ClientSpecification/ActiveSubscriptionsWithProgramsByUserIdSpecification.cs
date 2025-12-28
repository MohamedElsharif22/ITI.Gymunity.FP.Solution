using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using ITI.Gymunity.FP.Domain.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Specefications.ClientSpecification
{
    internal class ActiveSubscriptionsWithProgramsByUserIdSpecification : BaseSpecification<Subscription>
    {
        public ActiveSubscriptionsWithProgramsByUserIdSpecification(string userId)
            : base(s => s.ClientId == userId && s.Status == SubscriptionStatus.Active)
        {
            AddInclude(q => q.Include(s => s.Package)
                .ThenInclude(p => p.PackagePrograms)
                .ThenInclude(p => p.Program));
        }
    }
}
