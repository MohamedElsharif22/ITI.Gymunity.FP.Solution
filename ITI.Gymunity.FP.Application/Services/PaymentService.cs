using AutoMapper;
using ITI.Gymunity.FP.Application.Common;
using ITI.Gymunity.FP.Application.DTOs.User.Payment;
using ITI.Gymunity.FP.Application.Specefications.Payment;
using ITI.Gymunity.FP.Application.Specefications.Subscriptions;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Enums;
using Microsoft.Extensions.Logging;

namespace ITI.Gymunity.FP.Application.Services
{
    public class PaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentService> _logger;
        private readonly PayPalService _paypalService;

        public PaymentService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<PaymentService> logger,
            PayPalService paypalService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _paypalService = paypalService;
        }

        public async Task<ServiceResult<PaymentResponse>> InitiatePaymentAsync(
            string clientId,
            InitiatePaymentRequest request)
        {
            try
            {
                // 1. Validate Subscription
                var subSpec = new ClientSubscriptionByIdSpecs(request.SubscriptionId, clientId);
                var subscription = await _unitOfWork
                    .Repository<Subscription>()
                    .GetWithSpecsAsync(subSpec);

                if (subscription == null)
                    return ServiceResult<PaymentResponse>.Failure("Subscription not found");

                if (subscription.Status == SubscriptionStatus.Active)
                    return ServiceResult<PaymentResponse>.Failure("Subscription is already active and paid");

                if (subscription.Status == SubscriptionStatus.Canceled)
                    return ServiceResult<PaymentResponse>.Failure("Cannot pay for a canceled subscription");

                // 2. Check existing pending payment
                var existingPaymentSpec = new PaymentBySubscriptionSpecs(subscription.Id);
                var existingPayments = await _unitOfWork
                    .Repository<Payment>()
                    .GetAllWithSpecsAsync(existingPaymentSpec);

                var pendingPayment = existingPayments
                    .FirstOrDefault(p => p.Status == PaymentStatus.Pending
                                      || p.Status == PaymentStatus.Processing);

                if (pendingPayment != null)
                {
                    var existingResponse = _mapper.Map<PaymentResponse>(pendingPayment);
                    existingResponse.PaymentUrl = GeneratePaymentUrl(pendingPayment);
                    return ServiceResult<PaymentResponse>.Success(existingResponse);
                }

                // 3. Create new Payment
                var payment = new Payment
                {
                    SubscriptionId = request.SubscriptionId,
                    ClientId = clientId,
                    Amount = subscription.AmountPaid,
                    Currency = subscription.Currency,
                    Status = PaymentStatus.Pending,
                    Method = request.PaymentMethod,
                    CreatedAt = DateTime.UtcNow
                };

                payment.CalculateFees(subscription.PlatformFeePercentage);

                _unitOfWork.Repository<Payment>().Add(payment);
                await _unitOfWork.CompleteAsync();

                // 4. Generate payment URL
                string? paymentUrl = null;

                switch (request.PaymentMethod)
                {
                    case PaymentMethod.Paymob:
                        payment.PaymobOrderId = $"ORDER_{payment.Id}_{DateTime.UtcNow.Ticks}";
                        paymentUrl = await GeneratePaymobPaymentUrlAsync(payment, request.ReturnUrl);
                        break;

                    case PaymentMethod.PayPal:
                        var returnUrl = request.ReturnUrl ?? "http://localhost:5000/api/payment/paypal/return";
                        var cancelUrl = "http://localhost:5000/api/payment/paypal/cancel";

                        var paypalResult = await _paypalService.CreateOrderAsync(
                            payment,
                            returnUrl,
                            cancelUrl);

                        if (!paypalResult.Success)
                        {
                            _logger.LogError(
                                "Failed to create PayPal order for payment {PaymentId}: {Error}",
                                payment.Id,
                                paypalResult.ErrorMessage);

                            return ServiceResult<PaymentResponse>.Failure(
                                $"Failed to create PayPal order: {paypalResult.ErrorMessage}");
                        }

                        payment.PayPalOrderId = paypalResult.OrderId;
                        paymentUrl = paypalResult.ApprovalUrl;
                        break;

                    default:
                        return ServiceResult<PaymentResponse>.Failure("Payment method not supported");
                }

                await _unitOfWork.CompleteAsync();

                // 5. Return response
                var response = _mapper.Map<PaymentResponse>(payment);
                response.PaymentUrl = paymentUrl;

                _logger.LogInformation(
                    "Payment {PaymentId} initiated for subscription {SubscriptionId} by client {ClientId}",
                    payment.Id,
                    request.SubscriptionId,
                    clientId);

                return ServiceResult<PaymentResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error initiating payment for subscription {SubscriptionId}",
                    request.SubscriptionId);

                return ServiceResult<PaymentResponse>.Failure(
                    "An error occurred while initiating payment");
            }
        }

        public async Task<ServiceResult<bool>> ConfirmPaymentAsync(
            int paymentId,
            string transactionId)
        {
            try
            {
                var payment = await _unitOfWork
                    .Repository<Payment>()
                    .GetByIdAsync(paymentId);

                if (payment == null)
                    return ServiceResult<bool>.Failure("Payment not found");

                if (payment.Status == PaymentStatus.Completed)
                {
                    _logger.LogWarning(
                        "Payment {PaymentId} already completed. Ignoring duplicate.",
                        paymentId);
                    return ServiceResult<bool>.Success(true);
                }

                payment.Status = PaymentStatus.Completed;
                payment.PaidAt = DateTime.UtcNow;

                if (payment.Method == PaymentMethod.Paymob)
                    payment.PaymobTransactionId = transactionId;
                else if (payment.Method == PaymentMethod.PayPal)
                    payment.PayPalCaptureId = transactionId;

                var subscription = await _unitOfWork
                    .Repository<Subscription>()
                    .GetByIdAsync(payment.SubscriptionId);

                if (subscription != null)
                {
                    subscription.Status = SubscriptionStatus.Active;

                    if (payment.Method == PaymentMethod.Paymob)
                    {
                        subscription.PaymobOrderId = payment.PaymobOrderId;
                        subscription.PaymobTransactionId = transactionId;
                    }
                    else if (payment.Method == PaymentMethod.PayPal)
                    {
                        subscription.PayPalSubscriptionId = payment.PayPalOrderId;
                    }
                }

                await _unitOfWork.CompleteAsync();

                _logger.LogInformation(
                    "Payment {PaymentId} confirmed. Subscription {SubscriptionId} activated.",
                    paymentId,
                    payment.SubscriptionId);

                return ServiceResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error confirming payment {PaymentId}",
                    paymentId);

                return ServiceResult<bool>.Failure(
                    "An error occurred while confirming payment");
            }
        }

        public async Task<ServiceResult<bool>> FailPaymentAsync(
            int paymentId,
            string reason)
        {
            try
            {
                var payment = await _unitOfWork
                    .Repository<Payment>()
                    .GetByIdAsync(paymentId);

                if (payment == null)
                    return ServiceResult<bool>.Failure("Payment not found");

                payment.Status = PaymentStatus.Failed;
                payment.FailedAt = DateTime.UtcNow;
                payment.FailureReason = reason;

                await _unitOfWork.CompleteAsync();

                _logger.LogWarning(
                    "Payment {PaymentId} failed. Reason: {Reason}",
                    paymentId,
                    reason);

                return ServiceResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error marking payment {PaymentId} as failed",
                    paymentId);

                return ServiceResult<bool>.Failure(
                    "An error occurred while processing payment failure");
            }
        }

        public async Task<ServiceResult<PaymentHistoryResponse>> GetPaymentHistoryAsync(
            string clientId,
            PaymentStatus? status = null)
        {
            try
            {
                var spec = new ClientPaymentsSpecs(clientId, status);
                var payments = await _unitOfWork
                    .Repository<Payment>()
                    .GetAllWithSpecsAsync(spec);

                var paymentsList = payments.ToList();
                var response = new PaymentHistoryResponse
                {
                    TotalPayments = paymentsList.Count,
                    TotalAmount = paymentsList
                        .Where(p => p.Status == PaymentStatus.Completed)
                        .Sum(p => p.Amount),
                    Payments = paymentsList.Select(p => _mapper.Map<PaymentResponse>(p)).ToList()
                };

                return ServiceResult<PaymentHistoryResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error fetching payment history for client {ClientId}",
                    clientId);

                return ServiceResult<PaymentHistoryResponse>.Failure(
                    "An error occurred while fetching payment history");
            }
        }

        public async Task<ServiceResult<PaymentResponse>> GetPaymentByIdAsync(
            int id,
            string clientId)
        {
            try
            {
                var spec = new PaymentByIdSpecs(id, clientId);
                var payment = await _unitOfWork
                    .Repository<Payment>()
                    .GetWithSpecsAsync(spec);

                if (payment == null)
                    return ServiceResult<PaymentResponse>.Failure("Payment not found");

                var response = _mapper.Map<PaymentResponse>(payment);
                response.PaymentUrl = GeneratePaymentUrl(payment);

                return ServiceResult<PaymentResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error fetching payment {PaymentId} for client {ClientId}",
                    id,
                    clientId);

                return ServiceResult<PaymentResponse>.Failure(
                    "An error occurred while fetching payment details");
            }
        }

        #region Private Helper Methods

        private string GeneratePaymentUrl(Payment payment)
        {
            return payment.Method switch
            {
                PaymentMethod.Paymob => $"https://paymob.com/checkout/{payment.PaymobOrderId}",
                PaymentMethod.PayPal => $"https://www.sandbox.paypal.com/checkoutnow?token={payment.PayPalOrderId}",
                _ => string.Empty
            };
        }

        private async Task<string> GeneratePaymobPaymentUrlAsync(Payment payment, string? returnUrl)
        {
            // TODO: Implement Paymob integration
            await Task.CompletedTask;
            return $"https://paymob.com/iframe/{payment.PaymobOrderId}";
        }

        #endregion
    }
}