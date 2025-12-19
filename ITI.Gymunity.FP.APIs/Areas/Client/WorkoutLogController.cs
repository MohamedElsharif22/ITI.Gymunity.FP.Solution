using ITI.Gymunity.FP.APIs.Errors;
using ITI.Gymunity.FP.Application.DTOs.ClientDto;
using ITI.Gymunity.FP.Application.Services.ClientServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITI.Gymunity.FP.APIs.Areas.Client
{

    public class WorkoutLogController(WorkoutLogService workoutLogService) : ClientBaseController
    {
        private readonly WorkoutLogService _workoutLogService = workoutLogService;

        private string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }


        [HttpPost]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(WorkoutLogResponse), StatusCodes.Status201Created)]
        public async Task<ActionResult> AddAsync([FromBody] WorkoutLogRequest request)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ApiResponse(401, "Unauthorized"));

            try
            {
                var result = await _workoutLogService.AddWorkoutLogAsync(userId, request);

                return Created(nameof(GetWorkoutLogById), request);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, "An error occurred while creating workout log"));
            }
        }



        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(WorkoutLogResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetWorkoutLogById(long id)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ApiResponse(401, "Unauthorized"));

            var workoutLog = await _workoutLogService.GetWorkoutLogByIdAsync(userId, id);

            if (workoutLog == null)
                return NotFound(new ApiResponse(404, "WorkoutLog not found"));

            return Ok(workoutLog);
        }



        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<WorkoutLogResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetWorkoutLogs([FromQuery] int? pageNumber = null, [FromQuery] int? pageSize = null)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ApiResponse(401, "Unauthorized"));


            var workoutLogs = await _workoutLogService.GetWorkoutLogsByClientAsync(userId, pageNumber, pageSize);
            return Ok(workoutLogs);
        }



        [HttpPut("{id:long}")]
        [ProducesResponseType(typeof(WorkoutLogResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> updateWorkoutLogAsync(long id, [FromBody] WorkoutLogRequest request)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ApiResponse(401, "Unauthorized"));

            try
            {
                var response = await _workoutLogService.UpdateWorkoutLogAsync(userId, id, request);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, "An error occurred while updating workout log"));
            }
        }
        


        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteWorkoutLog(long id)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ApiResponse(401, "Unauthorized"));

            var result = await _workoutLogService.DeleteWorkoutLogAsync(userId, id);

            if (!result)
                return NotFound(new ApiResponse(404, "Workout log not found"));

            return NoContent();
        }
    }
}
