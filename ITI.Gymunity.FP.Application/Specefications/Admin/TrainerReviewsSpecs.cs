using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain.Specification;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ITI.Gymunity.FP.Application.Specefications.Admin
{
    /// <summary>
    /// Specification for retrieving reviews related to a specific trainer
    /// Includes approved reviews with client information
    /// </summary>
    public class TrainerReviewsSpecs : BaseSpecification<TrainerReview>
    {
        public TrainerReviewsSpecs(int trainerId, int pageNumber = 1, int pageSize = 10)
        {
            // Get reviews for this trainer that are approved
            Criteria = r => r.TrainerId == trainerId && r.IsApproved && !r.IsDeleted;

            // Eager load related client data
            AddInclude(r => r.Client);
            AddInclude(q => q.Include(r => r.Client).ThenInclude(c => c.User));

            // Sort by newest first
            AddOrderByDesc(r => r.CreatedAt);

            // Apply pagination
            ApplyPagination((pageNumber - 1) * pageSize, pageSize);
        }

        /// <summary>
        /// Constructor for getting all reviews (approved) for a trainer without pagination
        /// </summary>
        public TrainerReviewsSpecs(int trainerId)
        {
            // Get all approved reviews for this trainer
            Criteria = r => r.TrainerId == trainerId && r.IsApproved && !r.IsDeleted;

            // Eager load related client data
            AddInclude(r => r.Client);
            AddInclude(q => q.Include(r => r.Client).ThenInclude(c => c.User));

            // Sort by newest first
            AddOrderByDesc(r => r.CreatedAt);
        }
    }
}
