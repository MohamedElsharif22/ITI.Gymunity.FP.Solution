using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Specefications.ClientSpecification.Trainer_Discovery
{
    public class TrainerSearchSpecification : BaseSpecification<TrainerProfile>
    {
        public TrainerSearchSpecification(
            string? search = null,
            int? minExperience = null,
            bool? isVerified = null,
            bool isSuspended = false)
            : base(t =>
                (!isSuspended || !t.IsSuspended) &&
                (!isVerified.HasValue || t.IsVerified == isVerified.Value) &&
                (!minExperience.HasValue || t.YearsExperience >= minExperience.Value) &&
                (string.IsNullOrEmpty(search) ||
                 t.User.FullName.Contains(search) ||
                 t.Handle.Contains(search) ||
                 t.Bio.Contains(search)))
        {
            AddInclude(t => t.User);
            AddInclude(t => t.Packages);
        }
    }
}
