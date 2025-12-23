using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITI.Gymunity.FP.Domain.Specification;
using Microsoft.EntityFrameworkCore; 

namespace ITI.Gymunity.FP.Application.Specefications.Payment
{
    public class PaymentByIdSpecs : BaseSpecification<Domain.Models.Payment>
    {
        public PaymentByIdSpecs(int id, string clientId)
            : base(p => p.Id == id
                     && p.ClientId == clientId 
                     && !p.IsDeleted)
        {
            AddInclude(query => query
                .Include(p => p.Subscription)
                .ThenInclude(s => s.Package));
        }
    }
}
