using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain.Specification;
using Microsoft.EntityFrameworkCore;

namespace ITI.Gymunity.FP.Application.Specefications.Admin
{
    /// <summary>
    /// Specification for retrieving detailed information about a trainer
    /// Includes user data (email, username) and related information
    /// </summary>
    public class TrainerDetailSpecs : BaseSpecification<TrainerProfile>
    {
        public TrainerDetailSpecs(int trainerId)
        {
            // Filter by specific trainer ID
            Criteria = t => t.Id == trainerId && !t.IsDeleted;

            // Eager load user data (email, username, etc.)
            AddInclude(t => t.User);
        }
    }
}
