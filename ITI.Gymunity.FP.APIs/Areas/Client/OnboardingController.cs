using Azure.Messaging;
using ITI.Gymunity.FP.APIs.Responses;
using ITI.Gymunity.FP.Application.DTOs.ClientDto;
using ITI.Gymunity.FP.Application.Services.ClientServices;
using ITI.Gymunity.FP.Application.Specefications.ClientSpecification;
using ITI.Gymunity.FP.Domain.Models.Client;
using ITI.Gymunity.FP.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITI.Gymunity.FP.APIs.Areas.Client
{
    
    public class OnboardingController(OnboardingService onboardingService) : ClientBaseController
    {
        private readonly OnboardingService _onboardingService = onboardingService;

        private string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpPut("complete")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> CompleteProfileOnboardingAsync(OnboardingRequest request)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ApiResponse(401, "Unauthorized"));

            try
            {
                var result = await _onboardingService.CompleteOnboardingAsync(userId, request);

                if (!result)
                    return Conflict(new ApiResponse(409, "Profile already completed"));

                return Ok(new ApiResponse(200, "Your profile is completed"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }


        [HttpGet("status")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<bool>> IsProfileOnboardingCompleted()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ApiResponse(401, "Unauthorized"));

            var isCompleted = await _onboardingService.IsProfileOnboardingCompletedAsync(userId);

            return Ok(isCompleted);
        }
    }
}
