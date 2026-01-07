using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Specification;
using Microsoft.EntityFrameworkCore;
using DomainPayment = ITI.Gymunity.FP.Domain.Models.Payment;

namespace ITI.Gymunity.FP.Application.Specefications.Payment
{
    /// <summary>
    /// Find payment by Stripe Session ID
    /// Used for webhook processing and session verification
    /// </summary>
    public class PaymentByStripeSessionSpecs : BaseSpecification<DomainPayment>
    {
        public PaymentByStripeSessionSpecs(string sessionId)
            : base(p => p.StripeSessionId == sessionId && !p.IsDeleted)
        {
            // Include subscription with all related data for webhook processing
            AddInclude(q => q
                .Include(p => p.Subscription)
                    .ThenInclude(s => s.Package)
                        .ThenInclude(pkg => pkg.Trainer)
                            .ThenInclude(t => t.User));

            // Order by newest first
            AddOrderByDesc(p => p.CreatedAt);
        }
    }
}
