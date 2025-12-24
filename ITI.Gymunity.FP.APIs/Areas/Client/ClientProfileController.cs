using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.ClientDto;
using ITI.Gymunity.FP.Application.Services.ClientServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ITI.Gymunity.FP.APIs.Responses.Errors;
using ITI.Gymunity.FP.APIs.Responses;

namespace ITI.Gymunity.FP.APIs.Areas.Client
{
    public class ClientProfileController(ClientProfileService clientProfileService) : ClientBaseController
    {
        private readonly ClientProfileService _clientProfileService = clientProfileService;

        private string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpGet("dashboard")]
        [ProducesResponseType(typeof(ClientProfileDashboardResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClientProfileDashboardResponse>> GetDashboard()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ApiResponse(401, "Unauthorized"));

            try
            {
                var dashboard = await _clientProfileService.GetDashboardAsync(userId);
                return Ok(dashboard);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Dashboard not found for UserId: {UserId}", userId);
                return NotFound(new ApiResponse(404, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dashboard for UserId: {UserId}", userId);
                return StatusCode(500, new ApiResponse(500, "An error occurred while retrieving dashboard"));
            }
        }


        [HttpGet]
        [ProducesResponseType(typeof(ClientProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClientProfileResponse>> GetMyProfile()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ApiResponse(401, "Unauthorized"));

            var profile = await _clientProfileService.GetClientProfileAsync(userId);

            if (profile == null)
                return NotFound(new ApiResponse(404, "Client Profile not found"));

            return Ok(profile);
        }


        [HttpPost]
        [ProducesResponseType(typeof(ClientProfileResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> CreateClientProfile(ClientProfileRequest request)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ApiResponse(401, "Unauthorized"));

            var result = await _clientProfileService.CreateClientProfileAsync(userId, request);

            if (result == null)
                return Conflict(new ApiResponse(409, "Client profile already exists"));

            return Created(nameof(GetMyProfile), result);
        }


        [HttpPut]
        [ProducesResponseType(typeof(ClientProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClientProfileResponse>> UpdateMyProfile([FromBody] ClientProfileRequest request)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ApiResponse(401, "Unauthorized"));

            var updatedProfile = await _clientProfileService.UpdateClientProfileAsync(userId, request);

            if (updatedProfile == null)
                return NotFound(new ApiResponse(404, "Profile not found."));

            return Ok(updatedProfile);
        }



        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DeleteProfile()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ApiResponse(401, "Unauthorized"));

            var result = await _clientProfileService.DeleteProfileAsync(userId);

            if (!result)
                return NotFound(new ApiResponse(404, "Profile not found"));

            return NoContent();
        }
    }
}
