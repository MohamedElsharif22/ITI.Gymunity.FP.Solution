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
        private readonly IConfiguration _configuration;

        public PayPalCallbackController(
            PayPalService paypalService,
            PaymentService paymentService,
            IUnitOfWork unitOfWork,
            ILogger<PayPalCallbackController> logger,
            IConfiguration configuration)
        {
            _paypalService = paypalService;
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
            _logger = logger;
            this._configuration = configuration;
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
                    return Redirect($"{_configuration["FrontendOrigins:Hosted"] ?? _configuration["FrontendOrigins:Local"]}/payment/error");

                var capture = await _paypalService.CaptureOrderAsync(token);

                if (!capture.Success)
                {
                    await _paymentService.FailPaymentAsync(
                        payment.Id,
                        capture.ErrorMessage ?? "Capture failed");

                    return Redirect($"{_configuration["FrontendOrigins:Hosted"] ?? _configuration["FrontendOrigins:Local"]}/payment/failed");
                }

                await _paymentService.ConfirmPaymentAsync(
                    payment.Id, capture.CaptureId!);

                return Redirect(
                    $"{_configuration["FrontendOrigins:Hosted"] ?? _configuration["FrontendOrigins:Local"]}/payment/success?subscriptionId={payment.SubscriptionId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PayPal return error");
                return Redirect($"{_configuration["FrontendOrigins:Hosted"] ?? _configuration["FrontendOrigins:Local"]}/payment/error");
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

            return Redirect($"{_configuration["FrontendOrigins:Hosted"] ?? _configuration["FrontendOrigins:Local"]}/payment/canceled");
        }
    }
}
