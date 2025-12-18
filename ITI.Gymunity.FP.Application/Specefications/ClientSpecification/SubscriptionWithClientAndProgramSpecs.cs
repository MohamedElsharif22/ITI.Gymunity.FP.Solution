using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Specefications.ClientSpecification
{
    public class SubscriptionWithClientAndProgramSpecs : BaseSpecification<Subscription>
    {
        public SubscriptionWithClientAndProgramSpecs()
        {
            AddInclude(s => s.Package);
            AddInclude(s => s.Client);
        }

        public SubscriptionWithClientAndProgramSpecs(Expression<Func<Subscription, bool>>? criteria) : base(criteria)
        {
            AddInclude(s => s.Package);
            AddInclude(s => s.Client);
        }
    }
}
