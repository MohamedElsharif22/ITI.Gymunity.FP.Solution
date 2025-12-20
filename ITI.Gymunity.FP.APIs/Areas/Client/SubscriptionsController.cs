using ITI.Gymunity.FP.APIs.Errors;
using ITI.Gymunity.FP.Application.DTOs.User.Subscribe;
using ITI.Gymunity.FP.Application.Services;
using ITI.Gymunity.FP.Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.APIs.Areas.Client
{
    [Route("api/client/subscriptions")]  
    [Produces("application/json")]
    public class SubscriptionsController : ClientBaseController 
    {
        private readonly SubscriptionService _service;
        private readonly ILogger<SubscriptionsController> _logger;

        public SubscriptionsController(
            SubscriptionService service,
            ILogger<SubscriptionsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// Subscribe to a trainer's package
        [HttpPost("subscribe")]
        [ProducesResponseType(typeof(SubscriptionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Subscribe([FromBody] SubscribePackageRequest request)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.SubscribeAsync(clientId, request);

            if (!result.IsSuccess)
                return BadRequest(new ApiResponse(400, result.ErrorMessage));

            _logger.LogInformation(
                "Client {ClientId} subscribed to package {PackageId}",
                clientId,
                request.PackageId);

            return Ok(result.Data);
        }

        /// Get all my subscriptions
        [HttpGet]
        [ProducesResponseType(typeof(SubscriptionListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMySubscriptions(
            [FromQuery] SubscriptionStatus? status = null)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.GetMySubscriptionsAsync(clientId, status);

            if (!result.IsSuccess)
                return BadRequest(new ApiResponse(400, result.ErrorMessage));

            var subscriptions = result.Data!.ToList();
            var response = new SubscriptionListResponse
            {
                Subscriptions = subscriptions,
                TotalSubscriptions = subscriptions.Count,
                ActiveSubscriptions = subscriptions
                    .Count(s => s.Status == SubscriptionStatus.Active)
            };

            return Ok(response);
        }

        /// Get single subscription by ID
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(SubscriptionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSubscription(int id)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.GetSubscriptionByIdAsync(id, clientId);

            if (!result.IsSuccess)
                return NotFound(new ApiResponse(404, result.ErrorMessage));

            return Ok(result.Data);
        }

        /// Cancel subscription (keeps access until period end per SRS UC-10)
        [HttpPost("{id:int}/cancel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Cancel(int id)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.CancelAsync(id, clientId);

            if (!result.IsSuccess)
                return BadRequest(new ApiResponse(400, result.ErrorMessage));

            _logger.LogInformation(
                "Client {ClientId} canceled subscription {SubscriptionId}",
                clientId,
                id);

            return NoContent();
        }

        /// Reactivate a canceled subscription (if not expired)
        [HttpPost("{id:int}/reactivate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Reactivate(int id)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.ReactivateAsync(id, clientId);

            if (!result.IsSuccess)
                return BadRequest(new ApiResponse(400, result.ErrorMessage));

            _logger.LogInformation(
                "Client {ClientId} reactivated subscription {SubscriptionId}",
                clientId,
                id);

            return NoContent();
        }

        /// Check if client has access to a specific trainer's content
        [HttpGet("access/trainer/{trainerId}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> HasAccessToTrainer(string trainerId)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var hasAccess = await _service.HasActiveSubscriptionToTrainerAsync(
                clientId,
                trainerId);

            return Ok(new
            {
                hasAccess,
                trainerId,
                message = hasAccess
                    ? "You have active access to this trainer"
                    : "You don't have access to this trainer"
            });
        }
    }
}