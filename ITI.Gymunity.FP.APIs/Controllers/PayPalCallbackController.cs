using ITI.Gymunity.FP.Application.Services;
using ITI.Gymunity.FP.Application.Specefications.Payment;
using ITI.Gymunity.FP.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

        [HttpGet("return")]
        public async Task<IActionResult> PayPalReturn([FromQuery] string token)
        {
            try
            {
                var spec = PaymentByGatewayOrderSpecs.ForPayPal(token);

                var payment = await _unitOfWork
                    .Repository<Domain.Models.Payment>()
                    .GetWithSpecsAsync(spec);

                if (payment == null)
                    return Redirect("http://localhost:3000/payment/error");

                var capture = await _paypalService.CaptureOrderAsync(token);

                if (!capture.Success)
                {
                    await _paymentService.FailPaymentAsync(
                        payment.Id,
                        capture.ErrorMessage ?? "Capture failed");

                    return Redirect("http://localhost:3000/payment/failed");
                }

                await _paymentService.ConfirmPaymentAsync(
                    payment.Id, capture.CaptureId!);

                return Redirect(
                    $"http://localhost:3000/payment/success?subscriptionId={payment.SubscriptionId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PayPal return error");
                return Redirect("http://localhost:3000/payment/error");
            }
        }

        [HttpGet("cancel")]
        public async Task<IActionResult> PayPalCancel([FromQuery] string token)
        {
            var spec = PaymentByGatewayOrderSpecs.ForPayPal(token);

            var payment = await _unitOfWork
                .Repository<Domain.Models.Payment>()
                .GetWithSpecsAsync(spec);

            if (payment != null)
            {
                await _paymentService.FailPaymentAsync(
                    payment.Id, "User canceled payment");
            }

            return Redirect("http://localhost:3000/payment/canceled");
        }
    }
}
