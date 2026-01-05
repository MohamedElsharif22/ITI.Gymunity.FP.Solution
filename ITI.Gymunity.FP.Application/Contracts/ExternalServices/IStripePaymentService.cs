using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITI.Gymunity.FP.Domain.Models;

namespace ITI.Gymunity.FP.Application.Contracts.ExternalServices
{
    public interface IStripePaymentService
    {
        /// <summary>
        /// Creates a Stripe payment intent for a subscription payment
        /// Uses Subscription context instead of Payment to align with new architecture
        /// </summary>
        Task<(bool Success, string? ClientSecret, string? SessionId, string? ErrorMessage)>
            CreatePaymentIntentAsync(ITI.Gymunity.FP.Domain.Models.Subscription subscription, string returnUrl);

        /// <summary>
        /// Confirms a Stripe payment intent
        /// </summary>
        Task<(bool Success, string? PaymentIntentId, string? ErrorMessage)>
            ConfirmPaymentIntentAsync(string clientSecret);

        /// <summary>
        /// Retrieves Stripe payment intent details from a subscription
        /// </summary>
        Task<(bool Success, string? Status, decimal? Amount, string? ErrorMessage)>
            GetPaymentIntentAsync(string paymentIntentId);

        /// <summary>
        /// Refunds a Stripe payment
        /// </summary>
        Task<(bool Success, string? RefundId, string? ErrorMessage)>
            RefundPaymentAsync(string paymentIntentId, decimal? amount = null);

        /// <summary>
        /// Verifies webhook signature from Stripe
        /// </summary>
        bool VerifyWebhookSignature(string payload, string signature);
    }
}
