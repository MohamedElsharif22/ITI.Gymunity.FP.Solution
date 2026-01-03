using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using ITI.Gymunity.FP.Domain.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ITI.Gymunity.FP.Application.Specefications.Admin
{
    /// <summary>
    /// Specification for filtering programs in admin panel
    /// </summary>
    public class ProgramFilterSpecs : BaseSpecification<Program>
    {
        public ProgramFilterSpecs(
            bool? isPublic = null,
            ProgramType? programType = null,
            int? trainerProfileId = null,
            int pageNumber = 1,
            int pageSize = 10,
            string? searchTerm = null)
            : base(p => (!isPublic.HasValue || isPublic.Value == p.IsPublic)
                     && (!programType.HasValue || programType.Value == p.Type)
                     && (!trainerProfileId.HasValue || trainerProfileId.Value == p.TrainerProfileId)
                     && (string.IsNullOrEmpty(searchTerm) 
                        || EF.Functions.Like(p.Title, $"%{searchTerm}%")
                        || EF.Functions.Like(p.Description, $"%{searchTerm}%")
                     ))
        {
            // Include related data
            AddInclude(p => p.TrainerProfile!.User);
            AddInclude(p => p.Weeks);

            // Pagination
            ApplyPagination((pageNumber - 1) * pageSize, pageSize);

            // Default ordering - most recent first
            AddOrderByDesc(p => p.CreatedAt);
        }

        /// <summary>
        /// Constructor for count operations (no pagination)
        /// </summary>
        public ProgramFilterSpecs()
        {
        }
    }
}
