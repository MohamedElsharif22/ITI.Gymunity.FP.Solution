using ITI.Gymunity.FP.Application.DTOs.User.Subscribe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITI.Gymunity.FP.APIs.Areas.Client
{
    [Authorize(Roles = "Client")]
    [ApiController]
    [Route("api/client/subscriptions")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly SubscriptionService _service;

        public SubscriptionsController(SubscriptionService service)
        {
            _service = service;
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe(SubscribePackageRequest request)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var result = await _service.SubscribeAsync(clientId, request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMySubscriptions()
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var result = await _service.GetMySubscriptionsAsync(clientId);
            return Ok(result);
        }

        [HttpPost("{id:int}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            await _service.CancelAsync(id, clientId);
            return NoContent();
        }
    }

}
