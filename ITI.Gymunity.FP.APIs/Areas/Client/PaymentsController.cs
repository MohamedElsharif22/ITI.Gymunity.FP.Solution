using ITI.Gymunity.FP.APIs.Responses;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Infrastructure.DTOs.User.Payment;
using ITI.Gymunity.FP.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.APIs.Areas.Client
{
    [Route("api/client/payments")]
    [Produces("application/json")]
    public class PaymentsController : ClientBaseController
    {
        private readonly PaymentService _service;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(
            PaymentService service,
            ILogger<PaymentsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// Initiate payment for a subscription
        [HttpPost("initiate")]
        [ProducesResponseType(typeof(PaymentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InitiatePayment([FromBody] InitiatePaymentRequest request)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.InitiatePaymentAsync(clientId, request);

            if (!result.IsSuccess)
                return BadRequest(new ApiResponse(400, result.ErrorMessage));

            _logger.LogInformation(
                "Client {ClientId} initiated payment for subscription {SubscriptionId}",
                clientId,
                request.SubscriptionId);

            return Ok(result.Data);
        }

        /// Get payment history
        [HttpGet]
        [ProducesResponseType(typeof(PaymentHistoryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPaymentHistory(
            [FromQuery] PaymentStatus? status = null)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.GetPaymentHistoryAsync(clientId, status);

            if (!result.IsSuccess)
                return BadRequest(new ApiResponse(400, result.ErrorMessage));

            return Ok(result.Data);
        }

        /// Get payment details by ID
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(PaymentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPayment(int id)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.GetPaymentByIdAsync(id, clientId);

            if (!result.IsSuccess)
                return NotFound(new ApiResponse(404, result.ErrorMessage));

            return Ok(result.Data);
        }
    }
}