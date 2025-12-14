using ITI.Gymunity.FP.Application.DTOs.User.Subscribe;
using ITI.Gymunity.FP.Application.Services;
using ITI.Gymunity.FP.Domain.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.APIs.Areas.Client
{
    [Authorize(Roles = "Client")]
    [ApiController]
    [Route("api/client/subscriptions")]
    [Produces("application/json")]
    public class SubscriptionsController : ControllerBase
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

        /// <summary>
        /// Subscribe to a trainer's package
        /// </summary>
        /// <param name="request">Package subscription details</param>
        /// <returns>Created subscription</returns>
        [HttpPost("subscribe")]
        [ProducesResponseType(typeof(SubscriptionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Subscribe([FromBody] SubscribePackageRequest request)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.SubscribeAsync(clientId, request);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.ErrorMessage });

            _logger.LogInformation(
                "Client {ClientId} subscribed to package {PackageId}",
                clientId,
                request.PackageId);

            return Ok(result.Data);
        }

        /// <summary>
        /// Get all my subscriptions
        /// </summary>
        /// <param name="status">Optional filter by status</param>
        /// <returns>List of subscriptions</returns>
        [HttpGet]
        [ProducesResponseType(typeof(SubscriptionListResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMySubscriptions(
            [FromQuery] SubscriptionStatus? status = null)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.GetMySubscriptionsAsync(clientId, status);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.ErrorMessage });

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

        /// <summary>
        /// Get single subscription by ID
        /// </summary>
        /// <param name="id">Subscription ID</param>
        /// <returns>Subscription details</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(SubscriptionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSubscription(int id)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.GetSubscriptionByIdAsync(id, clientId);

            if (!result.IsSuccess)
                return NotFound(new { error = result.ErrorMessage });

            return Ok(result.Data);
        }

        /// <summary>
        /// Cancel subscription (keeps access until period end)
        /// </summary>
        /// <param name="id">Subscription ID</param>
        /// <returns>No content</returns>
        [HttpPost("{id:int}/cancel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Cancel(int id)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.CancelAsync(id, clientId);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.ErrorMessage });

            _logger.LogInformation(
                "Client {ClientId} canceled subscription {SubscriptionId}",
                clientId,
                id);

            return NoContent();
        }

        /// <summary>
        /// Reactivate a canceled subscription (if not expired)
        /// </summary>
        /// <param name="id">Subscription ID</param>
        /// <returns>No content</returns>
        [HttpPost("{id:int}/reactivate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Reactivate(int id)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.ReactivateAsync(id, clientId);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.ErrorMessage });

            _logger.LogInformation(
                "Client {ClientId} reactivated subscription {SubscriptionId}",
                clientId,
                id);

            return NoContent();
        }

        /// <summary>
        /// Check if client has access to a specific trainer
        /// </summary>
        /// <param name="trainerId">Trainer User ID</param>
        /// <returns>Access status</returns>
        [HttpGet("access/trainer/{trainerId}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> HasAccessToTrainer(string trainerId)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var hasAccess = await _service.HasActiveSubscriptionToTrainerAsync(
                clientId,
                trainerId);

            return Ok(new { hasAccess, trainerId });
        }
    }
}