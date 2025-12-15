using ITI.Gymunity.FP.Domain.Models.Client;
using ITI.Gymunity.FP.Domain.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Specefications.ClientSpecification
{
    public class ClientWithUserSpecs : BaseSpecification<ClientProfile>
    {
        public ClientWithUserSpecs()
        {
            AddInclude(c => c.User);
            AddInclude(c => c.Include(b => b.BodyStatLogs));
        }
        public ClientWithUserSpecs(Expression<Func<ClientProfile, bool>>? criteriaExpression) : base(criteriaExpression)
        {
            AddInclude(t => t.User);
            AddInclude(c => c.Include(b => b.BodyStatLogs));
        }
    }
}
