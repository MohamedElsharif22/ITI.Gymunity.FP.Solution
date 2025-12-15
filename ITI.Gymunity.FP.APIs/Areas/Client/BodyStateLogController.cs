using ITI.Gymunity.FP.APIs.Errors;
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
            if (userId == null)
            {
                return Unauthorized(new ApiResponse(401));
            }

            var result = await _bodyStateLogService.AddAsync(userId, request);
            return StatusCode(StatusCodes.Status201Created, result);
        }
    }
}
