using ITI.Gymunity.FP.Domain.Models.Client;
using ITI.Gymunity.FP.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Specefications.ClientSpecification
{
    public class BodyStateLogWithClientAsync : BaseSpecification<BodyStatLog>
    {
        public BodyStateLogWithClientAsync()
        {
            AddInclude(b => b.ClientProfile);
        }

        public BodyStateLogWithClientAsync(Expression<Func<BodyStatLog, bool>>? criteria) : base(criteria) 
        {
            AddInclude(b => b.ClientProfile);
        }
    }
}
