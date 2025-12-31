using ITI.Gymunity.FP.Application.DTOs.User.Subscribe;
using ITI.Gymunity.FP.Application.Services;
using ITI.Gymunity.FP.Domain.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITI.Gymunity.FP.APIs.Areas.Client
{
    public class SubscriptionsController : ClientBaseController
    {
        private readonly SubscriptionService _subscriptionService;

        public SubscriptionsController(SubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        /// <summary>
        /// Subscribe to a package (creates UNPAID subscription)
        /// POST: api/client/subscriptions/subscribe
        /// </summary>
        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe(
            [FromBody] SubscribePackageRequest request)
        {
            // 🔹 مؤقت لحد auth - هيبقى من الـ JWT Token
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? "b8f4c7e9-1c1f-4c5c-a12d-9a8f12345678";

            var result = await _subscriptionService
                .SubscribeAsync(clientId, request);

            if (!result.IsSuccess)
                return BadRequest(new
                {
                    success = false,
                    message = result.ErrorMessage
                });

            return Ok(new
            {
                success = true,
                message = "Subscription created successfully. Please proceed to payment.",
                data = result.Data
            });
        }

        /// <summary>
        /// Get all client subscriptions
        /// GET: api/client/subscriptions
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMySubscriptions(
            [FromQuery] SubscriptionStatus? status = null)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? "b8f4c7e9-1c1f-4c5c-a12d-9a8f12345678";

            var result = await _subscriptionService
                .GetClientSubscriptionsAsync(clientId, status);

            if (!result.IsSuccess)
                return BadRequest(new
                {
                    success = false,
                    message = result.ErrorMessage
                });

            return Ok(new
            {
                success = true,
                data = result.Data
            });
        }

        /// <summary>
        /// Get single subscription details
        /// GET: api/client/subscriptions/{id}
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubscriptionById(int id)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? "b8f4c7e9-1c1f-4c5c-a12d-9a8f12345678";

            var result = await _subscriptionService
                .GetSubscriptionByIdAsync(id, clientId);

            if (!result.IsSuccess)
                return NotFound(new
                {
                    success = false,
                    message = result.ErrorMessage
                });

            return Ok(new
            {
                success = true,
                data = result.Data
            });
        }

        /// <summary>
        /// Cancel subscription
        /// DELETE: api/client/subscriptions/{id}
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelSubscription(int id)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? "b8f4c7e9-1c1f-4c5c-a12d-9a8f12345678";

            var result = await _subscriptionService
                .CancelSubscriptionAsync(id, clientId);

            if (!result.IsSuccess)
                return BadRequest(new
                {
                    success = false,
                    message = result.ErrorMessage
                });

            return Ok(new
            {
                success = true,
                message = "Subscription canceled successfully"
            });
        }

        /// <summary>
        /// Activate subscription after payment (called by payment webhook)
        /// POST: api/client/subscriptions/{id}/activate
        /// </summary>
        [HttpPost("{id}/activate")]
        public async Task<IActionResult> ActivateSubscription(
            int id,
            [FromBody] ActivateSubscriptionRequest request)
        {
            var result = await _subscriptionService
                .ActivateSubscriptionAsync(id, request.TransactionId);

            if (!result.IsSuccess)
                return BadRequest(new
                {
                    success = false,
                    message = result.ErrorMessage
                });

            return Ok(new
            {
                success = true,
                message = "Subscription activated successfully",
                data = result.Data
            });
        }
    }

    // DTO for activation
    public class ActivateSubscriptionRequest
    {
        public string TransactionId { get; set; } = null!;
    }
}