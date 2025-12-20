using Microsoft.Extensions.Configuration;
using ITI.Gymunity.FP.Infrastructure.DTOs.User.Webhook;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Enums;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace ITI.Gymunity.FP.Infrastructure.Services
{
    public class WebhookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<WebhookService> _logger;
        private readonly IConfiguration _configuration;

        public WebhookService(
            IUnitOfWork unitOfWork,
            ILogger<WebhookService> logger,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Process Paymob webhook
        /// </summary>
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

                // 4. Check if already processed
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
                        .Repository<Subscription>()
                        .GetByIdAsync(payment.SubscriptionId);

                    if (subscription != null)
                    {
                        subscription.Status = SubscriptionStatus.Active;
                    }

                    await _unitOfWork.CompleteAsync();

                    _logger.LogInformation(
                        "Payment {PaymentId} completed successfully via Paymob",
                        paymentId);

                    // TODO: Send confirmation email to client
                    // TODO: Send notification to trainer

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
                    payment.FailureReason = "Transaction failed";

                    await _unitOfWork.CompleteAsync();

                    _logger.LogWarning(
                        "Payment {PaymentId} failed via Paymob",
                        paymentId);

                    return new WebhookResponse
                    {
                        Success = false,
                        Message = "Payment failed",
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

        /// <summary>
        /// Process PayPal webhook
        /// </summary>
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

                // 5. Check if already processed
                if (payment.Status == PaymentStatus.Completed)
                {
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
                }

                await _unitOfWork.CompleteAsync();

                _logger.LogInformation(
                    "Payment {PaymentId} completed successfully via PayPal",
                    paymentId);

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
        /// Verify Paymob HMAC signature
        /// </summary>
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
    }
}