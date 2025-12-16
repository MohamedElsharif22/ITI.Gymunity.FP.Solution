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
    public class WorkoutLogWithDetailsSpecs : BaseSpecification<WorkoutLog>
    {
        public WorkoutLogWithDetailsSpecs()
        {
            AddInclude(w => w.ClientProfile);
            AddInclude(w => w.ProgramDay);
            AddOrderByDesc(w => w.CompletedAt);
        }

        public WorkoutLogWithDetailsSpecs(Expression<Func<WorkoutLog, bool>> criteria)
        : base(criteria)
        {
            AddInclude(w => w.ClientProfile);
            AddInclude(w => w.ProgramDay);
            AddOrderByDesc(w => w.CompletedAt);
        }
    }
}
