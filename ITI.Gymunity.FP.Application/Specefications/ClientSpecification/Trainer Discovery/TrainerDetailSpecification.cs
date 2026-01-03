using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Specefications.ClientSpecification.Trainer_Discovery
{
    public class TrainerDetailSpecification : BaseSpecification<TrainerProfile>
    {
        public TrainerDetailSpecification(string trainerId)
            : base(t => t.UserId == trainerId)
        {
            AddInclude(t => t.User);
            AddInclude(t => t.Packages);
            AddInclude(t => t.Programs);
            AddInclude(t => t.Programs.Select(p => p.Weeks));
            AddInclude(t => t.TrainerReviews);
            AddInclude(t => t.TrainerReviews.Select(r => r.Client));
        }
    }
}
