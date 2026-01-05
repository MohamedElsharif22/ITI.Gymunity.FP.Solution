using ITI.Gymunity.FP.Admin.MVC.Services;
using ITI.Gymunity.FP.APIs.Responses;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Application.DTOs.User.Payment;
using ITI.Gymunity.FP.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITI.Gymunity.FP.APIs.Areas.Client
{
    public class PaymentsController : ClientBaseController
    {
        private readonly PaymentService _service;
        private readonly IAdminNotificationService _adminNotificationService;
        private readonly AdminUserResolverService _adminUserResolver;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(
            PaymentService service,
            IAdminNotificationService adminNotificationService,
            AdminUserResolverService adminUserResolver,
            ILogger<PaymentsController> logger)
        {
            _service = service;
            _adminNotificationService = adminNotificationService;
            _adminUserResolver = adminUserResolver;
            _logger = logger;
        }

        /// <summary>
        /// Initiate payment for a subscription
        /// POST: /api/client/payments/initiate
        /// </summary>
        [HttpPost("initiate")]
        [ProducesResponseType(typeof(PaymentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InitiatePayment([FromBody] InitiatePaymentRequest request)
        {
            // Get client ID from JWT or use test ID
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? "b8f4c7e9-1c1f-4c5c-a12d-9a8f12345678";

            var result = await _service.InitiatePaymentAsync(clientId, request);

            if (!result.IsSuccess)
                return BadRequest(new ApiResponse(400, result.ErrorMessage));

            _logger.LogInformation(
                "Client {ClientId} initiated payment for subscription {SubscriptionId}",
                clientId,
                request.SubscriptionId);

            // ✅ Notify admin of new payment
            await NotifyAdminOfPaymentAsync(result.Data, clientId);

            return Ok(result.Data);
        }

        /// <summary>
        /// Get payment history for current client
        /// GET: /api/client/payments
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PaymentHistoryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPaymentHistory(
            [FromQuery] PaymentStatus? status = null)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? "b8f4c7e9-1c1f-4c5c-a12d-9a8f12345678";

            var result = await _service.GetPaymentHistoryAsync(clientId, status);

            if (!result.IsSuccess)
                return BadRequest(new ApiResponse(400, result.ErrorMessage));

            return Ok(result.Data);
        }

        /// <summary>
        /// Get payment details by ID
        /// GET: /api/client/payments/{id}
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(PaymentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPayment(int id)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? "b8f4c7e9-1c1f-4c5c-a12d-9a8f12345678";

            var result = await _service.GetPaymentByIdAsync(id, clientId);

            if (!result.IsSuccess)
                return NotFound(new ApiResponse(404, result.ErrorMessage));

            return Ok(result.Data);
        }

        /// <summary>
        /// Sends admin notification for new payment
        /// </summary>
        private async Task NotifyAdminOfPaymentAsync(PaymentResponse payment, string clientId)
        {
            try
            {
                if (payment == null)
                    return;

                var admin = await _adminUserResolver.GetPrimaryAdminAsync();
                if (admin == null)
                {
                    _logger.LogWarning("No admin user found to notify about payment");
                    return;
                }

                await _adminNotificationService.CreateAdminNotificationAsync(
                    adminUserId: admin.Id,
                    title: "New Payment Initiated",
                    message: $"Payment of {payment.Amount:C} has been initiated",
                    type: NotificationType.NewPayment,
                    relatedEntityId: payment.Id.ToString(),
                    broadcastToAll: true
                );

                _logger.LogInformation("Admin notified of new payment {PaymentId}", payment.Id);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to notify admin of payment");
                // Don't rethrow - payment operation already succeeded
            }
        }
    }
}