using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain.Specification;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        public TrainerWithUsersAndProgramsSpecs(Expression<Func<TrainerProfile, bool>>? criteria) : base(criteria)
        {
            AddInclude(t => t.User);
            //AddInclude(tp => tp.Programs);
            AddInclude(q => q.Include(tp => tp.Programs).ThenInclude(p => p.Weeks));
        }
    }
}