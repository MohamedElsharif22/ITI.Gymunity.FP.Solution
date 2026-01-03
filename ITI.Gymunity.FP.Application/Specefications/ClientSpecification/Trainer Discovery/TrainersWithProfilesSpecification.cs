using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Specefications.ClientSpecification
{
    public class TrainersWithProfilesSpecification : BaseSpecification<TrainerProfile>
    {
        public TrainersWithProfilesSpecification(bool? isVerified = null, bool? isSuspended = false)
            : base(t => (!isVerified.HasValue || t.IsVerified == isVerified.Value) &&
                        (!isSuspended.HasValue || t.IsSuspended == isSuspended.Value))
        {
            AddInclude(t => t.User);
            AddInclude(t => t.Packages);
            AddInclude(t => t.Programs);

            AddOrderByDesc(t => t.RatingAverage);
        }
    }
}
