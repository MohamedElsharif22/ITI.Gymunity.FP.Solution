using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Specefications
{
    internal class TrainerWithUsersAndProgramsSpecs : BaseSpecification<TrainerProfile>
    {
        public TrainerWithUsersAndProgramsSpecs()
        {
            AddInclude(t => t.User);
            //AddInclude(tp => tp.Programs);
            AddInclude(q => q.Include(tp => tp.Programs).ThenInclude(p => p.Weeks));
        }
        public TrainerWithUsersAndProgramsSpecs(Expression<Func<TrainerProfile , bool>>? criteria) :base(criteria)
        {
            AddInclude(t => t.User);
            //AddInclude(tp => tp.Programs);
            AddInclude(q => q.Include(tp => tp.Programs).ThenInclude(p => p.Weeks));
        }
    }
}
