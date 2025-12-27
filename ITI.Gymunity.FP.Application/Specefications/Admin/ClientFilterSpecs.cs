using ITI.Gymunity.FP.Domain.Models.Client;
using ITI.Gymunity.FP.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Specefications.Admin
{
    /// <summary>
    /// Specification for filtering and retrieving client profiles with pagination
    /// Supports filtering by active/inactive status and search
    /// </summary>
    public class ClientFilterSpecs : BaseSpecification<ClientProfile>
    {
        public ClientFilterSpecs(
            bool? isActive = null,
            int pageNumber = 1,
            int pageSize = 10,
            string? searchTerm = null)
        {
            // Build criteria based on filters
            Expression<Func<ClientProfile, bool>>? criteria = null;

            // Active status filter
            if (isActive.HasValue)
            {
                criteria = c => c.IsDeleted == !isActive.Value;
            }

            // Search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var search = searchTerm.ToLower();
                var searchCriteria = (Expression<Func<ClientProfile, bool>>)(c =>
                    c.User.FullName.ToLower().Contains(search) ||
                    c.User.Email.ToLower().Contains(search) ||
                    c.User.UserName.ToLower().Contains(search));

                if (criteria != null)
                {
                    var existingCriteria = criteria;
                    criteria = c => searchCriteria.Compile().Invoke(c) && existingCriteria.Compile().Invoke(c);
                }
                else
                {
                    criteria = searchCriteria;
                }
            }

            if (criteria != null)
            {
                Criteria = criteria;
            }

            // Eager load related data
            AddInclude(c => c.User);
            AddInclude(c => c.BodyStatLogs);

            // Sort by newest first
            AddOrderByDesc(c => c.CreatedAt);

            // Apply pagination
            ApplyPagination((pageNumber - 1) * pageSize, pageSize);
        }
    }
}
