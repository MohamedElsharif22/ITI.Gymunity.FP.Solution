using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Specefications.Admin
{
    /// <summary>
    /// Specification for filtering and retrieving payment records with pagination
    /// Supports filtering by payment status and date range
    /// </summary>
    public class PaymentFilterSpecs : BaseSpecification<Domain.Models.Payment>
    {
        public PaymentFilterSpecs(
            PaymentStatus? status = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            // Build criteria based on filters
            Expression<Func<Domain.Models.Payment, bool>>? criteria = null;

            // Status filter
            if (status.HasValue)
            {
                criteria = p => p.Status == status.Value;
            }

            // Start date filter
            if (startDate.HasValue)
            {
                var start = startDate.Value;
                var startCriteria = (Expression<Func<Domain.Models.Payment, bool>>)(p => p.CreatedAt >= start);

                if (criteria != null)
                {
                    var existingCriteria = criteria;
                    criteria = p => startCriteria.Compile().Invoke(p) && existingCriteria.Compile().Invoke(p);
                }
                else
                {
                    criteria = startCriteria;
                }
            }

            // End date filter
            if (endDate.HasValue)
            {
                var end = endDate.Value;
                var endCriteria = (Expression<Func<Domain.Models.Payment, bool>>)(p => p.CreatedAt <= end);

                if (criteria != null)
                {
                    var existingCriteria = criteria;
                    criteria = p => endCriteria.Compile().Invoke(p) && existingCriteria.Compile().Invoke(p);
                }
                else
                {
                    criteria = endCriteria;
                }
            }

            if (criteria != null)
            {
                Criteria = criteria;
            }

            // Eager load related data
            AddInclude(p => p.Subscription);
            AddInclude(q => q.Include(p => p.Subscription).ThenInclude(s => s.Client));
            AddInclude(q => q.Include(p => p.Subscription).ThenInclude(s => s.Package));

            // Sort by newest first
            AddOrderByDesc(p => p.CreatedAt);

            // Apply pagination
            ApplyPagination((pageNumber - 1) * pageSize, pageSize);
        }
    }
}
