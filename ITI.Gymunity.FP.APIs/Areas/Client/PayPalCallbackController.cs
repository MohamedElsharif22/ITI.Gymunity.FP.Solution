using ITI.Gymunity.FP.Application.Services;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.APIs.Controllers
{
    [ApiController]
    [Route("api/payment/paypal")]
    public class PayPalCallbackController : ControllerBase
    {
        private readonly PayPalService _paypalService;
        private readonly PaymentService _paymentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PayPalCallbackController> _logger;

        public PayPalCallbackController(
            PayPalService paypalService,
            PaymentService paymentService,
            IUnitOfWork unitOfWork,
            ILogger<PayPalCallbackController> logger)
        {
            _paypalService = paypalService;
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// PayPal return URL (after user approves payment)
        [HttpGet("return")]
        public async Task<IActionResult> PayPalReturn([FromQuery] string token)
        {
            try
            {
                _logger.LogInformation("PayPal return received with token: {Token}", token);

                // 1. Find payment by PayPal order ID
                var payments = await _unitOfWork
                    .Repository<Payment>()
                    .GetAllAsync();

                var payment = payments.FirstOrDefault(p => p.PayPalOrderId == token);

                if (payment == null)
                {
                    _logger.LogWarning("Payment not found for PayPal token: {Token}", token);
                    return Redirect("http://localhost:3000/payment/error");
                }

                // 2. Capture the order
                var captureResult = await _paypalService.CaptureOrderAsync(token);

                if (!captureResult.Success)
                {
                    _logger.LogError(
                        "Failed to capture PayPal order {OrderId}: {Error}",
                        token,
                        captureResult.ErrorMessage);

                    await _paymentService.FailPaymentAsync(
                        payment.Id,
                        captureResult.ErrorMessage ?? "Capture failed");

                    return Redirect("http://localhost:3000/payment/failed");
                }

                // 3. Confirm payment
                await _paymentService.ConfirmPaymentAsync(payment.Id, captureResult.CaptureId!);

                _logger.LogInformation(
                    "PayPal payment {PaymentId} completed successfully",
                    payment.Id);

                // 4. Redirect to success page
                return Redirect($"http://localhost:3000/payment/success?subscriptionId={payment.SubscriptionId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing PayPal return");
                return Redirect("http://localhost:3000/payment/error");
            }
        }

        /// PayPal cancel URL
        [HttpGet("cancel")]
        public async Task<IActionResult> PayPalCancel([FromQuery] string token)
        {
            _logger.LogInformation("PayPal payment canceled: {Token}", token);

            try
            {
                // Find and mark payment as canceled
                var payments = await _unitOfWork
                    .Repository<Payment>()
                    .GetAllAsync();

                var payment = payments.FirstOrDefault(p => p.PayPalOrderId == token);

                if (payment != null)
                {
                    await _paymentService.FailPaymentAsync(payment.Id, "User canceled payment");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing PayPal cancel");
            }

            return Redirect("http://localhost:3000/payment/canceled");
        }
    }
}