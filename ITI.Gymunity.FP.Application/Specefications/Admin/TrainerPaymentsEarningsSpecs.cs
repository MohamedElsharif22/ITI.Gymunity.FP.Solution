using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Specification;
using Microsoft.EntityFrameworkCore;
using PaymentModel = ITI.Gymunity.FP.Domain.Models.Payment;

namespace ITI.Gymunity.FP.Application.Specefications.Admin
{
    /// <summary>
    /// Specification for retrieving completed payments for a trainer's packages
    /// Used to calculate total earnings and platform fees
    /// </summary>
    public class TrainerPaymentsEarningsSpecs : BaseSpecification<PaymentModel>
    {
        /// <summary>
        /// Get all completed payments for a specific trainer's packages
        /// </summary>
        public TrainerPaymentsEarningsSpecs(int trainerProfileId, bool allRecords = false)
        {
            // Get payments for subscriptions of packages belonging to this trainer
            // Status must be Completed for revenue counting
            Criteria = p => p.Status == PaymentStatus.Completed 
                && p.Subscription.Package.TrainerId == trainerProfileId 
                && !p.IsDeleted;

            // Eager load necessary data for calculations
            AddInclude(p => p.Subscription);
            AddInclude(q => q.Include(p => p.Subscription).ThenInclude(s => s.Package));

            // Sort by newest first
            AddOrderByDesc(p => p.CreatedAt);
        }
    }
}
