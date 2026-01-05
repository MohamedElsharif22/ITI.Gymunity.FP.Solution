using ITI.Gymunity.FP.Application.Configuration;
using ITI.Gymunity.FP.Application.Contracts.ExternalServices;
using ITI.Gymunity.FP.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainSubscription = ITI.Gymunity.FP.Domain.Models.Subscription;

namespace ITI.Gymunity.FP.Infrastructure.ExternalServices
{
    public class StripePaymentService : IStripePaymentService
    {
        private readonly StripeSettings _settings;
        private readonly ILogger<StripePaymentService> _logger;

        public StripePaymentService(
            IOptions<StripeSettings> settings,
            ILogger<StripePaymentService> logger)
        {
            _settings = settings.Value;
            _logger = logger;

            // Initialize Stripe API key
            StripeConfiguration.ApiKey = _settings.SecretKey;
        }

        /// <summary>
        /// Creates a Stripe payment intent for a subscription payment
        /// Now uses Subscription context instead of Payment
        /// </summary>
        public async Task<(bool Success, string? ClientSecret, string? SessionId, string? ErrorMessage)>
            CreatePaymentIntentAsync(DomainSubscription subscription, string returnUrl)
        {
            try
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(subscription.AmountPaid * 100), // Convert to cents
                    Currency = subscription.Currency.ToLower(),
                    Description = $"Subscription Payment - Package: {subscription.PackageId}",
                    Metadata = new Dictionary<string, string>
                    {
                        { "SubscriptionId", subscription.Id.ToString() },
                        { "ClientId", subscription.ClientId },
                        { "PackageId", subscription.PackageId.ToString() }
                    }
                };

                var service = new PaymentIntentService();
                var paymentIntent = await service.CreateAsync(options);

                _logger.LogInformation(
                    "Stripe payment intent created: {PaymentIntentId} for subscription {SubscriptionId}",
                    paymentIntent.Id,
                    subscription.Id);

                return (true, paymentIntent.ClientSecret, paymentIntent.Id, null);
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, 
                    "Stripe error creating payment intent for subscription {SubscriptionId}: {StripeErrorCode}",
                    subscription.Id,
                    ex.StripeError?.Code);

                return (false, null, null, ex.StripeError?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Stripe payment intent for subscription {SubscriptionId}", subscription.Id);
                return (false, null, null, ex.Message);
            }
        }

        /// <summary>
        /// Confirms a Stripe payment intent
        /// </summary>
        public async Task<(bool Success, string? PaymentIntentId, string? ErrorMessage)>
            ConfirmPaymentIntentAsync(string clientSecret)
        {
            try
            {
                // Extract payment intent ID from client secret
                var separator = "_secret_";
                var paymentIntentId = clientSecret.Split(new string[] { separator }, StringSplitOptions.None)[0];

                var service = new PaymentIntentService();
                var paymentIntent = await service.GetAsync(paymentIntentId);

                // Check if payment intent is already succeeded
                if (paymentIntent.Status == "succeeded")
                {
                    _logger.LogInformation(
                        "Stripe payment intent {PaymentIntentId} confirmed successfully",
                        paymentIntentId);

                    return (true, paymentIntentId, null);
                }

                return (false, null, $"Payment intent status is {paymentIntent.Status}");
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex,
                    "Stripe error confirming payment intent: {StripeErrorCode}",
                    ex.StripeError?.Code);

                return (false, null, ex.StripeError?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming Stripe payment intent");
                return (false, null, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves Stripe payment intent details
        /// </summary>
        public async Task<(bool Success, string? Status, decimal? Amount, string? ErrorMessage)>
            GetPaymentIntentAsync(string paymentIntentId)
        {
            try
            {
                var service = new PaymentIntentService();
                var paymentIntent = await service.GetAsync(paymentIntentId);

                var amountInDollars = paymentIntent.Amount / 100m;

                _logger.LogInformation(
                    "Retrieved Stripe payment intent {PaymentIntentId} with status {Status}",
                    paymentIntentId,
                    paymentIntent.Status);

                return (true, paymentIntent.Status, amountInDollars, null);
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex,
                    "Stripe error retrieving payment intent {PaymentIntentId}: {StripeErrorCode}",
                    paymentIntentId,
                    ex.StripeError?.Code);

                return (false, null, null, ex.StripeError?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Stripe payment intent {PaymentIntentId}", paymentIntentId);
                return (false, null, null, ex.Message);
            }
        }

        /// <summary>
        /// Refunds a Stripe payment
        /// </summary>
        public async Task<(bool Success, string? RefundId, string? ErrorMessage)>
            RefundPaymentAsync(string paymentIntentId, decimal? amount = null)
        {
            try
            {
                var options = new RefundCreateOptions
                {
                    PaymentIntent = paymentIntentId
                };

                if (amount.HasValue)
                {
                    options.Amount = (long)(amount.Value * 100); // Convert to cents
                }

                var service = new RefundService();
                var refund = await service.CreateAsync(options);

                _logger.LogInformation(
                    "Stripe refund created: {RefundId} for payment intent {PaymentIntentId}",
                    refund.Id,
                    paymentIntentId);

                return (true, refund.Id, null);
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex,
                    "Stripe error refunding payment intent {PaymentIntentId}: {StripeErrorCode}",
                    paymentIntentId,
                    ex.StripeError?.Code);

                return (false, null, ex.StripeError?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refunding Stripe payment intent {PaymentIntentId}", paymentIntentId);
                return (false, null, ex.Message);
            }
        }

        /// <summary>
        /// Verifies webhook signature from Stripe
        /// </summary>
        public bool VerifyWebhookSignature(string payload, string signature)
        {
            try
            {
                EventUtility.ValidateSignature(payload, signature, _settings.WebhookSecret);
                return true;
            }
            catch (StripeException ex)
            {
                _logger.LogWarning(ex, "Stripe webhook signature verification failed");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying Stripe webhook signature");
                return false;
            }
        }
    }
}
