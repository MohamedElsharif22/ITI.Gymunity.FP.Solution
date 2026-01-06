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
        /// Creates a Stripe Checkout Session for subscription payment
        /// Returns a URL to Stripe's hosted checkout page (similar to PayPal's approval URL)
        /// </summary>
        Task<(bool Success, string? CheckoutUrl, string? SessionId, string? ErrorMessage)>
            CreateCheckoutSessionAsync(ITI.Gymunity.FP.Domain.Models.Subscription subscription, string returnUrl, string cancelUrl);

        /// <summary>
        /// Retrieves a Checkout Session by ID
        /// </summary>
        Task<(bool Success, string? Status, string? PaymentIntentId, string? ErrorMessage)>
            GetCheckoutSessionAsync(string sessionId);

        /// <summary>
        /// Verifies webhook signature from Stripe
        /// </summary>
        bool VerifyWebhookSignature(string payload, string signature);

        /// <summary>
        /// Refunds a Stripe payment
        /// </summary>
        Task<(bool Success, string? RefundId, string? ErrorMessage)>
            RefundPaymentAsync(string paymentIntentId, decimal? amount = null);
    }
}
