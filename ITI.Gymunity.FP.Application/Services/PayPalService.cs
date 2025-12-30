using ITI.Gymunity.FP.Application.Configuration;
using ITI.Gymunity.FP.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Services
{
    public class PayPalService
    {
        private readonly PayPalSettings _settings;
        private readonly ILogger<PayPalService> _logger;
        private readonly PayPalHttpClient _client;

        public PayPalService(
            IOptions<PayPalSettings> settings,
            ILogger<PayPalService> logger)
        {
            _settings = settings.Value;
            _logger = logger;

            // Initialize PayPal client
            PayPalEnvironment environment;

            if (_settings.Mode == "Live")
            {
                environment = new LiveEnvironment(_settings.ClientId, _settings.ClientSecret);
            }
            else
            {
                environment = new SandboxEnvironment(_settings.ClientId, _settings.ClientSecret);
            }

            _client = new PayPalHttpClient(environment);
        }

        /// Create PayPal order
        public async Task<(bool Success, string? OrderId, string? ApprovalUrl, string? ErrorMessage)>
            CreateOrderAsync(Payment payment, string returnUrl, string cancelUrl)
        {
            try
            {
                var orderRequest = new OrderRequest
                {
                    CheckoutPaymentIntent = "CAPTURE",
                    PurchaseUnits = new List<PurchaseUnitRequest>
                    {
                        new PurchaseUnitRequest
                        {
                            ReferenceId = payment.Id.ToString(),
                            Description = $"Subscription Payment - ID: {payment.SubscriptionId}",
                            CustomId = payment.Id.ToString(),
                            SoftDescriptor = "TrainerHub",
                          AmountWithBreakdown = new AmountWithBreakdown
                        {
                           CurrencyCode = "USD",
                           Value = payment.Amount.ToString("F2")
                        }

                        }
                    },
                    ApplicationContext = new ApplicationContext
                    {
                        ReturnUrl = returnUrl,
                        CancelUrl = cancelUrl,
                        BrandName = "TrainerHub",
                        LandingPage = "BILLING",
                        UserAction = "PAY_NOW",
                        ShippingPreference = "NO_SHIPPING"
                    }
                };

                var request = new OrdersCreateRequest();
                request.Prefer("return=representation");
                request.RequestBody(orderRequest);

                var response = await _client.Execute(request);
                var result = response.Result<Order>();

                // Get approval URL
                var approvalUrl = result.Links
                    .FirstOrDefault(l => l.Rel == "approve")?.Href;

                _logger.LogInformation(
                    "PayPal order created: {OrderId} for payment {PaymentId}",
                    result.Id,
                    payment.Id);

                return (true, result.Id, approvalUrl, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating PayPal order for payment {PaymentId}", payment.Id);
                return (false, null, null, ex.Message);
            }
        }

        /// Capture PayPal order (after user approves)
        public async Task<(bool Success, string? CaptureId, string? ErrorMessage)>
            CaptureOrderAsync(string orderId)
        {
            try
            {
                var request = new OrdersCaptureRequest(orderId);
                request.Prefer("return=representation");
                request.RequestBody(new OrderActionRequest());

                var response = await _client.Execute(request);
                var result = response.Result<Order>();

                var captureId = result.PurchaseUnits[0]
                    .Payments.Captures[0].Id;

                _logger.LogInformation(
                    "PayPal order {OrderId} captured: {CaptureId}",
                    orderId,
                    captureId);

                return (true, captureId, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error capturing PayPal order {OrderId}", orderId);
                return (false, null, ex.Message);
            }
        }

        /// Get order details
        public async Task<Order?> GetOrderAsync(string orderId)
        {
            try
            {
                var request = new OrdersGetRequest(orderId);
                var response = await _client.Execute(request);
                return response.Result<Order>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting PayPal order {OrderId}", orderId);
                return null;
            }
        }

        /// Refund capture
        public async Task<(bool Success, string? RefundId, string? ErrorMessage)>
            RefundCaptureAsync(string captureId, decimal amount, string currency)
        {
            try
            {
                var request = new PayPalCheckoutSdk.Payments.CapturesRefundRequest(captureId);
                request.Prefer("return=representation");
                request.RequestBody(new PayPalCheckoutSdk.Payments.RefundRequest
                {
                    Amount = new PayPalCheckoutSdk.Payments.Money
                    {
                        CurrencyCode = currency,
                        Value = amount.ToString("F2")
                    }
                });

                var response = await _client.Execute(request);
                var result = response.Result<PayPalCheckoutSdk.Payments.Refund>();

                _logger.LogInformation(
                    "PayPal refund created: {RefundId} for capture {CaptureId}",
                    result.Id,
                    captureId);

                return (true, result.Id, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refunding PayPal capture {CaptureId}", captureId);
                return (false, null, ex.Message);
            }
        }
    }
}