using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Specification;
using Microsoft.EntityFrameworkCore; 

namespace ITI.Gymunity.FP.Infrastructure.Specefications.Payment
{
    public class ClientPaymentsSpecs : BaseSpecification<Domain.Models.Payment>
    {
        public ClientPaymentsSpecs(string clientId, PaymentStatus? status = null)
            : base(p => p.ClientId == clientId 
                     && !p.IsDeleted
                     && (!status.HasValue || p.Status == status.Value))
        {
            // Include relations for display
            AddInclude(query => query
                .Include(p => p.Subscription)
                .ThenInclude(s => s.Package));

            AddOrderByDesc(p => p.CreatedAt);
        }
    }
}