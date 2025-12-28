using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using ITI.Gymunity.FP.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Specefications.ProgramSpecs
{
    public class ProgramWeeksByProgramIdSpecification : BaseSpecification<ProgramWeek>
    {
        public ProgramWeeksByProgramIdSpecification(int programId)
            : base(pw => pw.ProgramId == programId)
        {
            AddInclude(pw => pw.Days);
        }
    }
}
