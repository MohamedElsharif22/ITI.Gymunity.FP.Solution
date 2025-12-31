
using ITI.Gymunity.FP.APIs.Responses;
using ITI.Gymunity.FP.APIs.Responses.Errors;
using ITI.Gymunity.FP.Application.DTOs.ClientDto;
using ITI.Gymunity.FP.Application.Services.ClientServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITI.Gymunity.FP.APIs.Areas.Client
{

    public class BodyStateLogController(BodyStateLogService bodyStateLogService) : ClientBaseController
    {
        private readonly BodyStateLogService _bodyStateLogService = bodyStateLogService;

        private string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BodyStateLogResponse), StatusCodes.Status201Created)]
        public async Task<ActionResult> AddAsync([FromBody] CreateBodyStateLogRequest request)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ApiResponse(401, "Unauthorized"));

            var result = await _bodyStateLogService.AddAsync(userId, request);
            return Created(nameof(GetLastStateLog), result);
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<BodyStateLogResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<BodyStateLogResponse>>> GetAllStateLogsByClient()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ApiResponse(401, "Unauthorized"));

            var result = await _bodyStateLogService.GetStateLogsByClientAsync(userId);

            return Ok(result);
        }


        [HttpGet("lastStateLog")]
        [ProducesResponseType(typeof(BodyStateLogResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public  async Task<ActionResult> GetLastStateLog()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ApiResponse(401, "Unauthorized"));

            var result = await _bodyStateLogService.GetLastStateLog(userId);
            return Ok(result);
        }

    }
}
