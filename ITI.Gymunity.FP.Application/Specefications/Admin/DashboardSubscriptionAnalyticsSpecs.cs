using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Specification;
using Microsoft.EntityFrameworkCore;
using System;

namespace ITI.Gymunity.FP.Application.Specefications.Admin
{
    /// <summary>
    /// Specification for dashboard subscription analytics
    /// Includes client, package, and trainer information for complete subscription analysis
    /// </summary>
    public class DashboardSubscriptionAnalyticsSpecs : BaseSpecification<Subscription>
    {
        /// <summary>
        /// Get all subscriptions with related data
        /// Optimized for dashboard analytics and aggregations
        /// </summary>
        public DashboardSubscriptionAnalyticsSpecs()
        {
            // Include client information
            AddInclude(q => q.Include(s => s.Client));

            // Include package and trainer data
            AddInclude(q => q.Include(s => s.Package)
                .ThenInclude(p => p.Trainer)
                .ThenInclude(t => t.User));

            // Include payments for revenue calculation
            AddInclude(q => q.Include(s => s.Payments));

            // Order by creation date
            AddOrderByDesc(s => s.CreatedAt);

            // No filtering - include all for analytics
        }

        /// <summary>
        /// Get subscriptions for a specific date range
        /// </summary>
        public DashboardSubscriptionAnalyticsSpecs(DateTime startDate, DateTime endDate)
            : this()
        {
            Criteria = s => s.CreatedAt >= startDate && s.CreatedAt <= endDate && !s.IsDeleted;
        }

        /// <summary>
        /// Get active subscriptions only
        /// </summary>
        public static DashboardSubscriptionAnalyticsSpecs ActiveSubscriptions()
        {
            var spec = new DashboardSubscriptionAnalyticsSpecs();
            spec.Criteria = s => s.Status == Domain.Models.Enums.SubscriptionStatus.Active && !s.IsDeleted;
            return spec;
        }

        /// <summary>
        /// Get unpaid subscriptions
        /// </summary>
        public static DashboardSubscriptionAnalyticsSpecs UnpaidSubscriptions()
        {
            var spec = new DashboardSubscriptionAnalyticsSpecs();
            spec.Criteria = s => s.Status == Domain.Models.Enums.SubscriptionStatus.Unpaid && !s.IsDeleted;
            return spec;
        }

        /// <summary>
        /// Get canceled subscriptions
        /// </summary>
        public static DashboardSubscriptionAnalyticsSpecs CanceledSubscriptions()
        {
            var spec = new DashboardSubscriptionAnalyticsSpecs();
            spec.Criteria = s => s.Status == Domain.Models.Enums.SubscriptionStatus.Canceled && !s.IsDeleted;
            return spec;
        }

        /// <summary>
        /// Get subscriptions by specific trainer
        /// </summary>
        public static DashboardSubscriptionAnalyticsSpecs ByTrainer(int trainerId)
        {
            var spec = new DashboardSubscriptionAnalyticsSpecs();
            spec.Criteria = s => s.Package.TrainerId == trainerId && !s.IsDeleted;
            return spec;
        }
    }
}
