using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using ITI.Gymunity.FP.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Specefications.ProgramSpecs
{
    internal class ProgramDaySpec : BaseSpecification<ProgramDay>
    {
        public ProgramDaySpec(int weekId)
            : base(d => d.ProgramWeekId == weekId)
        {
        }
        public ProgramDaySpec(Expression<Func<ProgramDay, bool>>? criteria)
            : base(criteria)
        {
        }
    }
}
