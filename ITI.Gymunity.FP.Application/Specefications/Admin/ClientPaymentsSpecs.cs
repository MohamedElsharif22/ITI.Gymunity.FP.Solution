using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace ITI.Gymunity.FP.Application.Specefications.Admin
{
    /// <summary>
    /// Specification for retrieving client payments with all related data
    /// Includes Subscription, Package, Trainer, and Trainer User information
    /// </summary>
    public class ClientPaymentsSpecs : BaseSpecification<ITI.Gymunity.FP.Domain.Models.Payment>
    {
        public ClientPaymentsSpecs(string clientId)
        {
            // Filter payments for subscriptions belonging to this client
            Criteria = p => p.Subscription != null && p.Subscription.ClientId == clientId;

            // Include all related data for complete payment information
            AddInclude(p => p.Subscription);
            AddInclude(p => p.Subscription.Package);
            AddInclude(p => p.Subscription.Package.Trainer);
            AddInclude(p => p.Subscription.Package.Trainer.User);

            // Sort by newest payments first
            AddOrderByDesc(p => p.CreatedAt);
        }
    }
}
