using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using ITI.Gymunity.FP.Application.DTOs.User.Webhook;
using ITI.Gymunity.FP.Application.Contracts.ExternalServices;
using ITI.Gymunity.FP.Application.DTOs.Email;
using ITI.Gymunity.FP.Application.Specefications.Payment;
using ITI.Gymunity.FP.Application.Specefications.Subscriptions;
using ITI.Gymunity.FP.Application.Contracts.Services;
using Stripe;
using Subscription = ITI.Gymunity.FP.Domain.Models.Subscription;
using ITI.Gymunity.FP.Application.Services;

namespace ITI.Gymunity.FP.Infrastructure.ExternalServices
{
    public class WebhookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<WebhookService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IStripePaymentService _stripePaymentService;
        private readonly EmailTemplateService _emailService;
        private readonly IAdminNotificationPublisher _adminNotificationPublisher;

        public WebhookService(
            IUnitOfWork unitOfWork,
            ILogger<WebhookService> logger,
            IConfiguration configuration,
            IStripePaymentService stripePaymentService,
            EmailTemplateService emailService,
            IAdminNotificationPublisher adminNotificationPublisher)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _configuration = configuration;
            _stripePaymentService = stripePaymentService;
            _emailService = emailService;
            _adminNotificationPublisher = adminNotificationPublisher;
        }

        /// Process Paymob webhook
        public async Task<WebhookResponse> ProcessPaymobWebhookAsync(
            PaymobWebhookPayload payload,
            string receivedHmac)
        {
            try
            {
                // 1. Verify HMAC signature
                var paymobSecret = _configuration["Paymob:HmacSecret"];
                if (!VerifyPaymobHmac(payload, receivedHmac, paymobSecret))
                {
                    _logger.LogWarning("Invalid Paymob HMAC signature");
                    return new WebhookResponse
                    {
                        Success = false,
                        Message = "Invalid signature"
                    };
                }

                // 2. Extract payment ID from merchant order ID
                if (!int.TryParse(payload.Obj.Order.Merchant_Order_Id, out int paymentId))
                {
                    _logger.LogWarning(
                        "Invalid merchant order ID: {OrderId}",
                        payload.Obj.Order.Merchant_Order_Id);
                    return new WebhookResponse
                    {
                        Success = false,
                        Message = "Invalid order ID"
                    };
                }

                // 3. Get payment from database
                var payment = await _unitOfWork
                    .Repository<Payment>()
                    .GetByIdAsync(paymentId);

                if (payment == null)
                {
                    _logger.LogWarning("Payment not found: {PaymentId}", paymentId);
                    return new WebhookResponse
                    {
                        Success = false,
                        Message = "Payment not found"
                    };
                }

                // 4. Check if already processed (idempotency)
                if (payment.Status == PaymentStatus.Completed)
                {
                    _logger.LogInformation(
                        "Payment {PaymentId} already processed",
                        paymentId);
                    return new WebhookResponse
                    {
                        Success = true,
                        Message = "Payment already processed",
                        PaymentId = paymentId
                    };
                }

                // 5. Update payment based on transaction success
                if (payload.Obj.Success)
                {
                    // Success: Update payment and subscription
                    payment.Status = PaymentStatus.Completed;
                    payment.PaidAt = DateTime.UtcNow;
                    payment.PaymobTransactionId = payload.Obj.Id.ToString();

                    // Update subscription status
                    var subscription = await _unitOfWork
                        .Repository<Domain.Models.Subscription>()
                        .GetByIdAsync(payment.SubscriptionId);

                    if (subscription != null)
                    {
                        subscription.Status = SubscriptionStatus.Active;
                        _unitOfWork.Repository<Domain.Models.Subscription>().Update(subscription);
                    }

                    _unitOfWork.Repository<Payment>().Update(payment);
                    await _unitOfWork.CompleteAsync();

                    _logger.LogInformation(
                        "✅ Paymob payment completed | Payment: {PaymentId}",
                        paymentId);

                    // Send confirmation emails
                    try
                    {
                        if (subscription != null)
                        {
                            await SendPaymobSuccessNotificationsAsync(subscription, payment);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to send email notifications for Paymob payment");
                        // Don't fail the webhook response - payment already processed
                    }

                    // ✅ Notify admins of new payment
                    try
                    {
                        var clientName = subscription?.Client?.FullName ?? "Unknown Client";
                        await _adminNotificationPublisher.NotifyNewPaymentAsync(
                            payment.Amount,
                            clientName,
                            paymentId.ToString());
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to publish admin notification for Paymob payment");
                        // Don't fail the webhook response
                    }

                    return new WebhookResponse
                    {
                        Success = true,
                        Message = "Payment completed successfully",
                        PaymentId = paymentId
                    };
                }
                else
                {
                    // Failed: Update payment status
                    payment.Status = PaymentStatus.Failed;
                    payment.FailureReason = "Paymob transaction failed";
                    payment.FailedAt = DateTime.UtcNow;

                    _unitOfWork.Repository<Payment>().Update(payment);
                    await _unitOfWork.CompleteAsync();

                    _logger.LogWarning(
                        "❌ Paymob payment failed | Payment: {PaymentId}",
                        paymentId);

                    // Send failure notification
                    try
                    {
                        var subscription = await _unitOfWork
                            .Repository<Domain.Models.Subscription>()
                            .GetByIdAsync(payment.SubscriptionId);

                        if (subscription != null)
                        {
                            await SendPaymobFailureNotificationAsync(subscription);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to send failure email for Paymob payment");
                        // Don't fail the webhook response - failure already processed
                    }

                    // ✅ Notify admins of payment failure
                    try
                    {
                        var subscription = await _unitOfWork
                            .Repository<Domain.Models.Subscription>()
                            .GetByIdAsync(payment.SubscriptionId);
                        var clientName = subscription?.Client?.FullName ?? "Unknown Client";
                        await _adminNotificationPublisher.NotifyPaymentFailureAsync(
                            payment.Amount,
                            clientName,
                            payment.FailureReason ?? "Paymob transaction failed",
                            paymentId.ToString());
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to publish admin failure notification for Paymob payment");
                        // Don't fail the webhook response
                    }

                    return new WebhookResponse
                    {
                        Success = true,
                        Message = "Payment failure processed",
                        PaymentId = paymentId
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Paymob webhook");
                return new WebhookResponse
                {
                    Success = false,
                    Message = "Internal error"
                };
            }
        }

        /// Process PayPal webhook
        public async Task<WebhookResponse> ProcessPayPalWebhookAsync(
            PayPalWebhookPayload payload)
        {
            try
            {
                // 1. Verify webhook signature (PayPal has its own verification API)
                // TODO: Implement PayPal signature verification

                // 2. Check event type
                if (payload.Event_Type != "PAYMENT.CAPTURE.COMPLETED")
                {
                    _logger.LogInformation(
                        "Ignored PayPal event type: {EventType}",
                        payload.Event_Type);
                    return new WebhookResponse
                    {
                        Success = true,
                        Message = "Event ignored"
                    };
                }

                // 3. Extract payment ID from reference
                var referenceId = payload.Resource.Purchase_Units[0].Reference_Id;
                if (!int.TryParse(referenceId, out int paymentId))
                {
                    _logger.LogWarning("Invalid PayPal reference ID: {ReferenceId}", referenceId);
                    return new WebhookResponse
                    {
                        Success = false,
                        Message = "Invalid reference ID"
                    };
                }

                // 4. Get payment from database
                var payment = await _unitOfWork
                    .Repository<Payment>()
                    .GetByIdAsync(paymentId);

                if (payment == null)
                {
                    _logger.LogWarning("Payment not found: {PaymentId}", paymentId);
                    return new WebhookResponse
                    {
                        Success = false,
                        Message = "Payment not found"
                    };
                }

                // 5. Check if already processed (idempotency)
                if (payment.Status == PaymentStatus.Completed)
                {
                    _logger.LogInformation(
                        "Payment {PaymentId} already processed",
                        paymentId);
                    return new WebhookResponse
                    {
                        Success = true,
                        Message = "Payment already processed",
                        PaymentId = paymentId
                    };
                }

                // 6. Update payment and subscription
                payment.Status = PaymentStatus.Completed;
                payment.PaidAt = DateTime.UtcNow;
                payment.PayPalOrderId = payload.Resource.Id;

                var subscription = await _unitOfWork
                    .Repository<Subscription>()
                    .GetByIdAsync(payment.SubscriptionId);

                if (subscription != null)
                {
                    subscription.Status = SubscriptionStatus.Active;
                    _unitOfWork.Repository<Subscription>().Update(subscription);
                }

                _unitOfWork.Repository<Payment>().Update(payment);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation(
                    "✅ PayPal payment completed | Payment: {PaymentId}",
                    paymentId);

                // Send confirmation emails
                try
                {
                    if (subscription != null)
                    {
                        await SendPayPalSuccessNotificationsAsync(subscription, payment);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to send email notifications for PayPal payment");
                    // Don't fail the webhook response - payment already processed
                }

                // ✅ Notify admins of new payment
                try
                {
                    var clientName = subscription?.Client?.FullName ?? "Unknown Client";
                    await _adminNotificationPublisher.NotifyNewPaymentAsync(
                        payment.Amount,
                        clientName,
                        paymentId.ToString());
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to publish admin notification for PayPal payment");
                    // Don't fail the webhook response
                }

                return new WebhookResponse
                {
                    Success = true,
                    Message = "Payment completed successfully",
                    PaymentId = paymentId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing PayPal webhook");
                return new WebhookResponse
                {
                    Success = false,
                    Message = "Internal error"
                };
            }
        }

        /// <summary>
        /// Process Stripe webhook events
        /// Handles: payment_intent.succeeded, payment_intent.payment_failed, payment_intent.canceled
        /// </summary>
        public async Task<WebhookResponse> ProcessStripeWebhookAsync(
            string jsonPayload,
            string signatureHeader)
        {
            try
            {
                // 1. Verify webhook signature
                if (!_stripePaymentService.VerifyWebhookSignature(jsonPayload, signatureHeader))
                {
                    _logger.LogWarning("Invalid Stripe webhook signature");
                    return new WebhookResponse
                    {
                        Success = false,
                        Message = "Invalid signature"
                    };
                }

                // 2. Parse event
                var @event = EventUtility.ParseEvent(jsonPayload);

                if (@event == null)
                {
                    _logger.LogWarning("Failed to parse Stripe webhook event");
                    return new WebhookResponse
                    {
                        Success = false,
                        Message = "Invalid event format"
                    };
                }

                _logger.LogInformation(
                    "📥 Stripe webhook received | Event: {EventType} | ID: {EventId}",
                    @event.Type,
                    @event.Id);

                // 3. Handle event types
                if (@event.Type == "payment_intent.succeeded")
                {
                    return await HandlePaymentIntentSucceededAsync(@event);
                }
                else if (@event.Type == "payment_intent.payment_failed")
                {
                    return await HandlePaymentIntentFailedAsync(@event);
                }
                else if (@event.Type == "payment_intent.canceled")
                {
                    return await HandlePaymentIntentCanceledAsync(@event);
                }
                else
                {
                    _logger.LogInformation(
                        "Ignored Stripe event type: {EventType}",
                        @event.Type);
                    return new WebhookResponse
                    {
                        Success = true,
                        Message = "Event ignored"
                    };
                }
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Stripe error processing webhook: {StripeErrorCode}", ex.StripeError?.Code);
                return new WebhookResponse
                {
                    Success = false,
                    Message = "Stripe error"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Stripe webhook");
                return new WebhookResponse
                {
                    Success = false,
                    Message = "Internal error"
                };
            }
        }

        /// <summary>
        /// Handle payment_intent.succeeded event
        /// </summary>
        private async Task<WebhookResponse> HandlePaymentIntentSucceededAsync(Event @event)
        {
            try
            {
                var paymentIntent = @event.Data.Object as PaymentIntent;

                if (paymentIntent == null)
                {
                    _logger.LogWarning("Failed to extract PaymentIntent from event");
                    return new WebhookResponse
                    {
                        Success = false,
                        Message = "Invalid payment intent"
                    };
                }

                // Extract subscription ID from metadata
                if (!paymentIntent.Metadata.TryGetValue("SubscriptionId", out var subscriptionIdStr) ||
                    !int.TryParse(subscriptionIdStr, out int subscriptionId))
                {
                    _logger.LogWarning(
                        "Invalid or missing SubscriptionId in metadata for payment intent {PaymentIntentId}",
                        paymentIntent.Id);
                    return new WebhookResponse
                    {
                        Success = false,
                        Message = "Invalid subscription reference"
                    };
                }

                // Get subscription with client info
                var subscription = await _unitOfWork
                    .Repository<Subscription>()
                    .GetByIdAsync(subscriptionId);

                if (subscription == null)
                {
                    _logger.LogWarning(
                        "Subscription not found for payment intent {PaymentIntentId}",
                        paymentIntent.Id);
                    return new WebhookResponse
                    {
                        Success = false,
                        Message = "Subscription not found"
                    };
                }

                // Get associated payment using spec
                var paymentSpec = new PaymentBySubscriptionSpecs(subscriptionId);
                var payment = await _unitOfWork
                    .Repository<Payment>()
                    .GetWithSpecsAsync(paymentSpec);


                if (payment != null && payment.Status == PaymentStatus.Completed)
                {
                    _logger.LogInformation(
                        "Payment for subscription {SubscriptionId} already processed",
                        subscriptionId);
                    return new WebhookResponse
                    {
                        Success = true,
                        Message = "Payment already processed",
                        PaymentId = payment.Id
                    };
                }

                // Update subscription status
                subscription.Status = SubscriptionStatus.Active;
                subscription.StripePaymentIntentId = paymentIntent.Id;

                if (payment != null)
                {
                    payment.Status = PaymentStatus.Completed;
                    payment.PaidAt = DateTime.UtcNow;
                    _unitOfWork.Repository<Payment>().Update(payment);
                }

                _unitOfWork.Repository<Subscription>().Update(subscription);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation(
                    "✅ Stripe payment succeeded | Subscription: {SubscriptionId} | PaymentIntent: {PaymentIntentId}",
                    subscriptionId,
                    paymentIntent.Id);

                // Send confirmation emails
                try
                {
                    await SendStripeSuccessNotificationsAsync(subscription, payment);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to send email notifications for Stripe payment");
                    // Don't fail the webhook response - payment already processed
                }

                // ✅ Notify admins of new payment
                try
                {
                    var clientName = subscription?.Client?.FullName ?? "Unknown Client";
                    var paymentAmount = payment?.Amount ?? subscription?.AmountPaid ?? 0;
                    var paymentId = payment?.Id.ToString() ?? subscriptionId.ToString();
                    
                    await _adminNotificationPublisher.NotifyNewPaymentAsync(
                        paymentAmount,
                        clientName,
                        paymentId);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to publish admin notification for Stripe payment");
                    // Don't fail the webhook response
                }

                return new WebhookResponse
                {
                    Success = true,
                    Message = "Payment completed successfully",
                    PaymentId = payment?.Id ?? 0
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling payment_intent.succeeded event");
                return new WebhookResponse
                {
                    Success = false,
                    Message = "Internal error"
                };
            }
        }

        /// <summary>
        /// Handle payment_intent.payment_failed event
        /// </summary>
        private async Task<WebhookResponse> HandlePaymentIntentFailedAsync(Event @event)
        {
            try
            {
                var paymentIntent = @event.Data.Object as PaymentIntent;

                if (paymentIntent == null)
                {
                    _logger.LogWarning("Failed to extract PaymentIntent from event");
                    return new WebhookResponse
                    {
                        Success = false,
                        Message = "Invalid payment intent"
                    };
                }

                // Extract subscription ID from metadata
                if (!paymentIntent.Metadata.TryGetValue("SubscriptionId", out var subscriptionIdStr) ||
                    !int.TryParse(subscriptionIdStr, out int subscriptionId))
                {
                    _logger.LogWarning(
                        "Invalid or missing SubscriptionId in metadata for payment intent {PaymentIntentId}",
                        paymentIntent.Id);
                    return new WebhookResponse
                    {
                        Success = false,
                        Message = "Invalid subscription reference"
                    };
                }

                // Get subscription
                var subscription = await _unitOfWork
                    .Repository<Subscription>()
                    .GetByIdAsync(subscriptionId);

                if (subscription == null)
                {
                    _logger.LogWarning(
                        "Subscription not found for payment intent {PaymentIntentId}",
                        paymentIntent.Id);
                    return new WebhookResponse
                    {
                        Success = false,
                        Message = "Subscription not found"
                    };
                }

                // Get associated payment using spec
                var paymentSpec = new PaymentBySubscriptionSpecs(subscriptionId);
                var payments = await _unitOfWork
                    .Repository<Payment>()
                    .GetAllWithSpecsAsync(paymentSpec);

                var payment = payments.FirstOrDefault();

                if (payment != null)
                {
                    payment.Status = PaymentStatus.Failed;
                    payment.FailureReason = paymentIntent.LastPaymentError?.Message ?? "Stripe payment failed";
                    payment.FailedAt = DateTime.UtcNow;
                    _unitOfWork.Repository<Payment>().Update(payment);
                }

                await _unitOfWork.CompleteAsync();

                _logger.LogWarning(
                    "❌ Stripe payment failed | Subscription: {SubscriptionId} | PaymentIntent: {PaymentIntentId} | Error: {ErrorMessage}",
                    subscriptionId,
                    paymentIntent.Id,
                    paymentIntent.LastPaymentError?.Message);

                // Send failure email
                try
                {
                    await SendStripeFailureNotificationAsync(
                        subscription,
                        paymentIntent.LastPaymentError?.Message ?? "Payment failed");
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to send failure email for Stripe payment");
                    // Don't fail the webhook response - failure already processed
                }

                // ✅ Notify admins of payment failure
                try
                {
                    var clientName = subscription?.Client?.FullName ?? "Unknown Client";
                    var paymentAmount = payment?.Amount ?? subscription?.AmountPaid ?? 0;
                    var failureReason = paymentIntent.LastPaymentError?.Message ?? "Stripe payment failed";
                    var paymentId = payment?.Id.ToString() ?? subscriptionId.ToString();
                    
                    await _adminNotificationPublisher.NotifyPaymentFailureAsync(
                        paymentAmount,
                        clientName,
                        failureReason,
                        paymentId);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to publish admin failure notification for Stripe payment");
                    // Don't fail the webhook response
                }

                return new WebhookResponse
                {
                    Success = true,
                    Message = "Payment failure processed",
                    PaymentId = payment?.Id ?? 0
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling payment_intent.payment_failed event");
                return new WebhookResponse
                {
                    Success = false,
                    Message = "Internal error"
                };
            }
        }

        /// <summary>
        /// Handle payment_intent.canceled event
        /// </summary>
        private async Task<WebhookResponse> HandlePaymentIntentCanceledAsync(Event @event)
        {
            try
            {
                var paymentIntent = @event.Data.Object as PaymentIntent;

                if (paymentIntent == null)
                {
                    _logger.LogWarning("Failed to extract PaymentIntent from event");
                    return new WebhookResponse
                    {
                        Success = false,
                        Message = "Invalid payment intent"
                    };
                }

                // Extract subscription ID from metadata
                if (!paymentIntent.Metadata.TryGetValue("SubscriptionId", out var subscriptionIdStr) ||
                    !int.TryParse(subscriptionIdStr, out int subscriptionId))
                {
                    _logger.LogWarning(
                        "Invalid or missing SubscriptionId in metadata for payment intent {PaymentIntentId}",
                        paymentIntent.Id);
                    return new WebhookResponse
                    {
                        Success = false,
                        Message = "Invalid subscription reference"
                    };
                }

                // Get subscription
                var subscription = await _unitOfWork
                    .Repository<Subscription>()
                    .GetByIdAsync(subscriptionId);

                if (subscription == null)
                {
                    _logger.LogWarning(
                        "Subscription not found for payment intent {PaymentIntentId}",
                        paymentIntent.Id);
                    return new WebhookResponse
                    {
                        Success = false,
                        Message = "Subscription not found"
                    };
                }

                // Get associated payment using spec
                var paymentSpec = new PaymentBySubscriptionSpecs(subscriptionId);
                var payments = await _unitOfWork
                    .Repository<Payment>()
                    .GetAllWithSpecsAsync(paymentSpec);

                var payment = payments.FirstOrDefault();

                if (payment != null)
                {
                    payment.Status = PaymentStatus.Failed;
                    payment.FailureReason = "Payment intent was canceled";
                    payment.FailedAt = DateTime.UtcNow;
                    _unitOfWork.Repository<Payment>().Update(payment);
                }

                await _unitOfWork.CompleteAsync();

                _logger.LogWarning(
                    "❌ Stripe payment canceled | Subscription: {SubscriptionId} | PaymentIntent: {PaymentIntentId}",
                    subscriptionId,
                    paymentIntent.Id);

                // Send cancellation email
                try
                {
                    await SendStripeFailureNotificationAsync(subscription, "Payment was canceled");
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to send cancellation email for Stripe payment");
                    // Don't fail the webhook response - cancellation already processed
                }

                // ✅ Notify admins of payment cancellation
                try
                {
                    var clientName = subscription?.Client?.FullName ?? "Unknown Client";
                    var paymentAmount = payment?.Amount ?? subscription?.AmountPaid ?? 0;
                    var paymentId = payment?.Id.ToString() ?? subscriptionId.ToString();
                    
                    await _adminNotificationPublisher.NotifyPaymentFailureAsync(
                        paymentAmount,
                        clientName,
                        "Payment intent was canceled",
                        paymentId);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to publish admin cancellation notification for Stripe payment");
                    // Don't fail the webhook response
                }

                return new WebhookResponse
                {
                    Success = true,
                    Message = "Payment cancellation processed",
                    PaymentId = payment?.Id ?? 0
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling payment_intent.canceled event");
                return new WebhookResponse
                {
                    Success = false,
                    Message = "Internal error"
                };
            }
        }

        /// <summary>
        /// Send success notifications for Stripe payment
        /// </summary>
        private async Task SendStripeSuccessNotificationsAsync(Subscription subscription, Payment? payment)
        {
            // Get complete subscription with client and package info
            var spec = new ClientSubscriptionByIdSpecs(subscription.Id, subscription.ClientId);
            var fullSubscription = await _unitOfWork
                .Repository<Subscription>()
                .GetWithSpecsAsync(spec);
            
            if (fullSubscription?.Client == null || fullSubscription.Package == null)
                return;

            var client = fullSubscription.Client;
            var package = fullSubscription.Package;

            // Get trainer info from package
            var trainer = package.Trainer;

            // Send client confirmation
            var clientEmailData = new SubscriptionConfirmationEmail
            {
                ClientName = client.UserName ?? "Client",
                ClientEmail = client.Email ?? "",
                PackageName = package.Name,
                TrainerName = trainer?.User?.UserName ?? "Coach",
                Amount = fullSubscription.AmountPaid,
                Currency = fullSubscription.Currency,
                SubscriptionStartDate = fullSubscription.StartDate,
                SubscriptionEndDate = fullSubscription.CurrentPeriodEnd
            };

            await _emailService.SendSubscriptionConfirmationAsync(clientEmailData);

            // Send trainer notification
            if (trainer?.User?.Email != null)
            {
                var trainerEmailData = new TrainerNotificationEmail
                {
                    TrainerName = trainer.User.UserName ?? "Coach",
                    TrainerEmail = trainer.User.Email,
                    ClientName = client.UserName ?? "Client",
                    PackageName = package.Name,
                    Amount = fullSubscription.AmountPaid,
                    TrainerPayout = payment?.TrainerPayout ?? (fullSubscription.AmountPaid * 0.85m),
                    Currency = fullSubscription.Currency
                };

                await _emailService.SendTrainerNotificationAsync(trainerEmailData);
            }
        }

        /// <summary>
        /// Send failure notification for Stripe payment
        /// </summary>
        private async Task SendStripeFailureNotificationAsync(Subscription subscription, string failureReason)
        {
            // Get complete subscription with client and package info
            var spec = new ClientSubscriptionByIdSpecs(subscription.Id, subscription.ClientId);
            var subscriptions = await _unitOfWork
                .Repository<Subscription>()
                .GetAllWithSpecsAsync(spec);
            
            var fullSubscription = subscriptions.FirstOrDefault();
            if (fullSubscription?.Client?.Email == null || fullSubscription.Package == null)
                return;

            var client = fullSubscription.Client;
            var package = fullSubscription.Package;

            await _emailService.SendPaymentFailureEmailAsync(
                client.Email,
                client.UserName ?? "Client",
                package.Name,
                failureReason);
        }

        /// Verify Paymob HMAC signature
        private bool VerifyPaymobHmac(
            PaymobWebhookPayload payload,
            string receivedHmac,
            string secret)
        {
            // Build the string to hash (based on Paymob documentation)
            var dataToHash = string.Join("",
                payload.Obj.Amount_Cents,
                payload.Obj.Created_At,
                payload.Obj.Currency,
                payload.Obj.Id,
                payload.Obj.Order.Merchant_Order_Id,
                payload.Obj.Success.ToString().ToLower()
            );

            // Calculate HMAC
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dataToHash));
            var calculatedHmac = BitConverter.ToString(hash).Replace("-", "").ToLower();

            return calculatedHmac == receivedHmac.ToLower();
        }

        /// <summary>
        /// Send success notifications for PayPal payment
        /// </summary>
        private async Task SendPayPalSuccessNotificationsAsync(Subscription subscription, Payment? payment)
        {
            try
            {
                // Get complete subscription with client and package info
                var spec = new ClientSubscriptionByIdSpecs(subscription.Id, subscription.ClientId);
                var subscriptions = await _unitOfWork
                    .Repository<Subscription>()
                    .GetAllWithSpecsAsync(spec);

                var fullSubscription = subscriptions.FirstOrDefault();
                if (fullSubscription?.Client == null || fullSubscription.Package == null)
                    return;

                var client = fullSubscription.Client;
                var package = fullSubscription.Package;
                var trainer = package.Trainer;

                // Send client confirmation
                var clientEmailData = new SubscriptionConfirmationEmail
                {
                    ClientName = client.UserName ?? "Client",
                    ClientEmail = client.Email ?? "",
                    PackageName = package.Name,
                    TrainerName = trainer?.User?.UserName ?? "Coach",
                    Amount = subscription.AmountPaid,
                    Currency = subscription.Currency,
                    SubscriptionStartDate = subscription.StartDate,
                    SubscriptionEndDate = subscription.CurrentPeriodEnd
                };

                await _emailService.SendSubscriptionConfirmationAsync(clientEmailData);

                // Send trainer notification
                if (trainer?.User?.Email != null)
                {
                    var trainerEmailData = new TrainerNotificationEmail
                    {
                        TrainerName = trainer.User.UserName ?? "Coach",
                        TrainerEmail = trainer.User.Email,
                        ClientName = client.UserName ?? "Client",
                        PackageName = package.Name,
                        Amount = subscription.AmountPaid,
                        TrainerPayout = payment?.TrainerPayout ?? (subscription.AmountPaid * 0.85m),
                        Currency = subscription.Currency
                    };

                    await _emailService.SendTrainerNotificationAsync(trainerEmailData);
                }

                _logger.LogInformation("Sent PayPal success notifications for subscription {SubscriptionId}", subscription.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending PayPal success notifications");
                throw;
            }
        }

        /// <summary>
        /// Send success notifications for Paymob payment
        /// </summary>
        private async Task SendPaymobSuccessNotificationsAsync(Subscription subscription, Payment? payment)
        {
            try
            {
                // Get complete subscription with client and package info
                var spec = new ClientSubscriptionByIdSpecs(subscription.Id, subscription.ClientId);
                var subscriptions = await _unitOfWork
                    .Repository<Subscription>()
                    .GetAllWithSpecsAsync(spec);

                var fullSubscription = subscriptions.FirstOrDefault();
                if (fullSubscription?.Client == null || fullSubscription.Package == null)
                    return;

                var client = fullSubscription.Client;
                var package = fullSubscription.Package;
                var trainer = package.Trainer;

                // Send client confirmation
                var clientEmailData = new SubscriptionConfirmationEmail
                {
                    ClientName = client.UserName ?? "Client",
                    ClientEmail = client.Email ?? "",
                    PackageName = package.Name,
                    TrainerName = trainer?.User?.UserName ?? "Coach",
                    Amount = subscription.AmountPaid,
                    Currency = subscription.Currency,
                    SubscriptionStartDate = subscription.StartDate,
                    SubscriptionEndDate = subscription.CurrentPeriodEnd
                };

                await _emailService.SendSubscriptionConfirmationAsync(clientEmailData);

                // Send trainer notification
                if (trainer?.User?.Email != null)
                {
                    var trainerEmailData = new TrainerNotificationEmail
                    {
                        TrainerName = trainer.User.UserName ?? "Coach",
                        TrainerEmail = trainer.User.Email,
                        ClientName = client.UserName ?? "Client",
                        PackageName = package.Name,
                        Amount = subscription.AmountPaid,
                        TrainerPayout = payment?.TrainerPayout ?? (subscription.AmountPaid * 0.85m),
                        Currency = subscription.Currency
                    };

                    await _emailService.SendTrainerNotificationAsync(trainerEmailData);
                }

                _logger.LogInformation("Sent Paymob success notifications for subscription {SubscriptionId}", subscription.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending Paymob success notifications");
                throw;
            }
        }

        /// <summary>
        /// Send failure notification for Paymob payment
        /// </summary>
        private async Task SendPaymobFailureNotificationAsync(Subscription subscription)
        {
            try
            {
                // Get complete subscription with client and package info
                var spec = new ClientSubscriptionByIdSpecs(subscription.Id, subscription.ClientId);
                var subscriptions = await _unitOfWork
                    .Repository<Subscription>()
                    .GetAllWithSpecsAsync(spec);

                var fullSubscription = subscriptions.FirstOrDefault();
                if (fullSubscription?.Client?.Email == null || fullSubscription.Package == null)
                    return;

                var client = fullSubscription.Client;
                var package = fullSubscription.Package;

                await _emailService.SendPaymentFailureEmailAsync(
                    client.Email,
                    client.UserName ?? "Client",
                    package.Name,
                    "Your Paymob payment could not be completed. Please try again.");

                _logger.LogInformation("Sent Paymob failure notification for subscription {SubscriptionId}", subscription.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending Paymob failure notification");
                throw;
            }
        }
    }
}