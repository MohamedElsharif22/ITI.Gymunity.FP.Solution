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
    public class SubscriptionsWithClientAndProgramSpecs : BaseSpecification<Subscription>
    {
        public SubscriptionsWithClientAndProgramSpecs()
        {
            AddInclude(s => s.Package);
            AddInclude(s => s.Client);
        }

        public SubscriptionsWithClientAndProgramSpecs(Expression<Func<Subscription, bool>>? criteria) : base(criteria)
        {
            AddInclude(s => s.Package);
            AddInclude(s => s.Client);
        }
    }
}
