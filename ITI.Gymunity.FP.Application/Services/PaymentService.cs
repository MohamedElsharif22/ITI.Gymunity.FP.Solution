using AutoMapper;
using ITI.Gymunity.FP.Application.Common;
using ITI.Gymunity.FP.Application.Contracts.ExternalServices;
using ITI.Gymunity.FP.Application.DTOs.User.Payment;
using ITI.Gymunity.FP.Application.Specefications.Payment;
using ITI.Gymunity.FP.Application.Specefications.Subscriptions;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ITI.Gymunity.FP.Application.Services
{
    public class PaymentService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IPayPalService paypalService,
        IStripePaymentService stripePaymentService,
        ILogger<PaymentService> logger,
        IConfiguration configuration)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IPayPalService _paypalService = paypalService;
        private readonly IStripePaymentService _stripePaymentService = stripePaymentService;
        private readonly ILogger<PaymentService> _logger = logger;
        private readonly IConfiguration _configuration = configuration;

        // Get frontend base URL from configuration
        private readonly string _frontendBaseUrl = configuration["FrontendOrigins:Hosted"]
                    ?? configuration["FrontendOrigins:Local"] ?? "https://localhost:4200";

        // ===============================
        // Initiate Payment
        // ===============================
        public async Task<ServiceResult<PaymentResponse>> InitiatePaymentAsync(
            string clientId,
            InitiatePaymentRequest request)
        {
            try
            {
                // 1️⃣ Validate subscription
                var subSpec = new ClientSubscriptionByIdSpecs(
                    request.SubscriptionId, clientId);

                var subscription = await _unitOfWork
                    .Repository<Subscription>()
                    .GetWithSpecsAsync(subSpec);

                if (subscription == null)
                    return ServiceResult<PaymentResponse>
                        .Failure("Subscription not found");

                if (subscription.Status == SubscriptionStatus.Active)
                    return ServiceResult<PaymentResponse>
                        .Failure("Subscription already active");

                if (subscription.Status == SubscriptionStatus.Canceled)
                    return ServiceResult<PaymentResponse>
                        .Failure("Subscription is canceled");

                // 2️⃣ Check existing pending payment
                var existingPayments = await _unitOfWork
                    .Repository<Payment>()
                    .GetAllWithSpecsAsync(
                        new PaymentBySubscriptionSpecs(subscription.Id,clientId));

                var pendingPayment = existingPayments.FirstOrDefault(p =>
                    p.Status == PaymentStatus.Pending ||
                    p.Status == PaymentStatus.Processing);

                if (pendingPayment != null)
                {
                    var pendingResponse = _mapper.Map<PaymentResponse>(pendingPayment);
                    return ServiceResult<PaymentResponse>.Success(pendingResponse);
                }

                // 3️⃣ Create payment record
                var payment = new Payment
                {
                    SubscriptionId = subscription.Id,
                    ClientId = clientId,
                    Amount = subscription.AmountPaid,
                    Currency = subscription.Currency ?? "USD",  // ✅ Use subscription currency instead of hardcoded USD
                    Method = request.PaymentMethod,
                    Status = PaymentStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                payment.CalculateFees(subscription.PlatformFeePercentage);

                _unitOfWork.Repository<Payment>().Add(payment);
                await _unitOfWork.CompleteAsync();

                // 4️⃣ Initiate gateway
                string? paymentUrl = null;

                if (request.PaymentMethod == PaymentMethod.PayPal)
                {
                    var returnUrl =
                        request.ReturnUrl ??
                        $"{_frontendBaseUrl}/payment/return?subscriptionId={subscription.Id}";

                    var cancelUrl =
                        $"{_frontendBaseUrl}/payment/cancel?subscriptionId={subscription.Id}";

                    // ✅ Pass subscription to PayPal service (new architecture)
                    var result = await _paypalService.CreateOrderAsync(
                        subscription,
                        returnUrl,
                        cancelUrl);

                    if (!result.Success)
                        return ServiceResult<PaymentResponse>
                            .Failure(result.ErrorMessage ?? "PayPal error");

                    // ✅ Store PayPal order on BOTH subscription AND payment
                    subscription.PayPalOrderId = result.OrderId;
                    subscription.PayPalApprovalUrl = result.ApprovalUrl;
                    
                    // ✅ NEW: Also set PayPal order ID on payment record
                    payment.PayPalOrderId = result.OrderId;
                    
                    paymentUrl = result.ApprovalUrl;

                    _unitOfWork.Repository<Subscription>().Update(subscription);
                    _unitOfWork.Repository<Payment>().Update(payment);
                }
                else if (request.PaymentMethod == PaymentMethod.Stripe)
                {
                    var returnUrl =
                        request.ReturnUrl ??
                        $"{_frontendBaseUrl}/payment/return?subscriptionId={subscription.Id}";

                    var cancelUrl =
                        $"{_frontendBaseUrl}/payment/cancel?subscriptionId={subscription.Id}";

                    // ✅ Create Stripe Checkout Session (aligned with PayPal approach)
                    var result = await _stripePaymentService.CreateCheckoutSessionAsync(
                        subscription,
                        returnUrl,
                        cancelUrl);

                    if (!result.Success)
                        return ServiceResult<PaymentResponse>
                            .Failure(result.ErrorMessage ?? "Stripe error");

                    // ✅ Store Stripe Session ID on subscription
                    subscription.StripePaymentIntentId = result.SessionId;
                    
                    // ✅ Store checkout URL for frontend redirect
                    paymentUrl = result.CheckoutUrl;

                    _unitOfWork.Repository<Subscription>().Update(subscription);
                }
                else
                {
                    return ServiceResult<PaymentResponse>
                        .Failure("Payment method not supported");
                }

                await _unitOfWork.CompleteAsync();

                // 5️⃣ Response
                var response = _mapper.Map<PaymentResponse>(payment);
                response.PaymentUrl = paymentUrl;

                return ServiceResult<PaymentResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Payment initiation failed");
                return ServiceResult<PaymentResponse>
                    .Failure("Failed to initiate payment");
            }
        }

        // ===============================
        // Confirm Payment (after PayPal/Stripe return)
        // ===============================
        public async Task ConfirmPaymentAsync(int paymentId, string captureId)
        {
            var payment = await _unitOfWork
                .Repository<Payment>()
                .GetByIdAsync(paymentId);

            if (payment == null) return;

            payment.Status = PaymentStatus.Completed;
            payment.PaidAt = DateTime.UtcNow;
            
            if (payment.Method == PaymentMethod.PayPal)
            {
                payment.PayPalCaptureId = captureId;
            }

            var subscription = await _unitOfWork
                .Repository<Subscription>()
                .GetByIdAsync(payment.SubscriptionId);

            if (subscription != null)
            {
                subscription.Status = SubscriptionStatus.Active;
                
                // ✅ Update subscription with payment intent if Stripe
                if (payment.Method == PaymentMethod.Stripe)
                {
                    subscription.StripePaymentIntentId = captureId;
                }
            }

            await _unitOfWork.CompleteAsync();
        }

        // ===============================
        // Fail Payment
        // ===============================
        public async Task FailPaymentAsync(int paymentId, string reason)
        {
            var payment = await _unitOfWork
                .Repository<Payment>()
                .GetByIdAsync(paymentId);

            if (payment == null) return;

            payment.Status = PaymentStatus.Failed;
            payment.FailureReason = reason;
            payment.FailedAt = DateTime.UtcNow;

            await _unitOfWork.CompleteAsync();
        }

        // ===============================
        // Get Payment History
        // ===============================
        public async Task<ServiceResult<PaymentHistoryResponse>>
            GetPaymentHistoryAsync(string clientId, PaymentStatus? status)
        {
            var payments = await _unitOfWork
                .Repository<Payment>()
                .GetAllWithSpecsAsync(
                    new ClientPaymentsSpecs(clientId, status));

            var list = payments
                .Select(p => _mapper.Map<PaymentResponse>(p))
                .ToList();

            return ServiceResult<PaymentHistoryResponse>.Success(
                new PaymentHistoryResponse
                {
                    TotalPayments = list.Count,
                    TotalAmount = list.Sum(p => p.Amount),
                    Payments = list
                });
        }

        public async Task<ServiceResult<PaymentResponse>> GetPaymentByIdAsync(
            int paymentId,
            string clientId)
        {
            var payment = await _unitOfWork
                .Repository<Payment>()
                .GetByIdAsync(paymentId);

            if (payment == null)
                return ServiceResult<PaymentResponse>.Failure("Payment not found");

            if (payment.ClientId != clientId)
                return ServiceResult<PaymentResponse>.Failure("Unauthorized access");

            var response = _mapper.Map<PaymentResponse>(payment);
            return ServiceResult<PaymentResponse>.Success(response);
        }
    }
}
