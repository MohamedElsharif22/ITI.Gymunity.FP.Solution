using ITI.Gymunity.FP.Domain.Models.Client;
using ITI.Gymunity.FP.Domain.Specification;
using System;
using System.Linq.Expressions;

namespace ITI.Gymunity.FP.Application.Specefications.Admin
{
    /// <summary>
    /// Specification for retrieving detailed client information including all related data
    /// Used for the client details/profile view with subscriptions and payments
    /// </summary>
    public class ClientDetailSpecs : BaseSpecification<ClientProfile>
    {
        public ClientDetailSpecs(string clientUserId)
        {
            // Filter by user ID
            Criteria = c => c.UserId == clientUserId;

            // Include all related data for complete client profile
            AddInclude(c => c.User);
            AddInclude(c => c.BodyStatLogs);
        }
    }
}
