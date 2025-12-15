using AutoMapper;
using ITI.Gymunity.FP.APIs.Errors;
using ITI.Gymunity.FP.Application.DTOs.ClientDto;
using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Application.Services.ClientServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ITI.Gymunity.FP.APIs.Areas.Client
{
    public class ClientProfileController(ClientProfileService clientProfileService) : ClientBaseController
    {
        private readonly ClientProfileService _clientProfileService = clientProfileService;

        private string? GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }


        [HttpGet("profile")]
        [ProducesResponseType(typeof(ClientProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientProfileResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ClientProfileResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClientProfileResponse>> GetMyProfile()
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            var profile = await _clientProfileService.GetClientProfileAsync(userId);

            if (profile == null)
                return NotFound("No client profile found.");

            return Ok(profile);
        }


        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateClientProfile(ClientProfileRequest request)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            var result = await _clientProfileService.CreateClientProfileAsync(userId, request);

            if (result == null)
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Unable to create Profile"));
            return Ok(new ApiResponse(200, "Success"));

        }


        [HttpPut("profile")]
        [ProducesResponseType(typeof(ClientProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClientProfileResponse>> UpdateMyProfile([FromBody] ClientProfileRequest request)
        {
            var userId = GetUserId();
            if (userId == null) 
                return Unauthorized();

            var updatedProfile = await _clientProfileService.UpdateClientProfileAsync(userId, request);

            if (updatedProfile == null)
                return NotFound("Profile not found.");

            return Ok(updatedProfile);
        }


        //[HttpPatch("profile/goal")]
        //[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult> UpdateMyGoal([FromBody] UpdateClientGoalRequest request)
        //{
        //    var userId = GetUserId();
        //    if (userId == null)
        //        return Unauthorized();

        //    var success = await _clientProfileService.UpdateClientGoalAsync(userId, request.Goal);

        //    if (!success)
        //        return NotFound("Profile not found.");

        //    return Ok(new { message = "Goal updated successfully." });
        //}


        //[HttpPatch("profile/photo")]
        //[ProducesResponseType(typeof(ClientProfileResponse), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult> UpdateMyPhoto([FromBody] UpdateProfilePhotoRequest request)
        //{
        //    var userId = GetUserId();
        //    if (userId == null)
        //        return Unauthorized();

        //    var success = await _clientProfileService.UpdateClientInfoAsync(userId, request.PhotoUrl);

        //    if (!success)
        //        return NotFound("Client profile not found.");

        //    var updatedProfile = await _clientProfileService.GetClientProfileAsync(userId);

        //    return Ok(updatedProfile);
        //}


        


        [HttpDelete("profile/delete")]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DeleteProfile(ClientProfileRequest request)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            var result = await _clientProfileService.DeleteProfileAsync(userId);

            if (!result) return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Unable to Delete Profile"));
            return Ok(new ApiResponse(200));
        }
    }
}
