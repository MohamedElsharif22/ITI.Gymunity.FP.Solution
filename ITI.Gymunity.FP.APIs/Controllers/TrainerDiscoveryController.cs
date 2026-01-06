using ITI.Gymunity.FP.Application.DTOs;
using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Application.Services.ClientServices;
using ITI.Gymunity.FP.APIs.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITI.Gymunity.FP.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainerDiscoveryController : ControllerBase
    {
        private readonly TrainerDiscoveryService _trainerService;

        public TrainerDiscoveryController(TrainerDiscoveryService trainerService)
        {
            _trainerService = trainerService;
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("User ID not found in token");
        }

        /// <summary>
        /// Search and browse all trainers with filters
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Pagination<TrainerCardDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult> SearchTrainers()
        {
            var clientId = GetCurrentUserId();

            var result = await _trainerService.SearchTrainersAsync();

            return Ok(result);
        }

        /// <summary>
        /// Get detailed profile of a specific trainer by userId
        /// </summary>
        /// <param name="trainerId">The trainer's user ID</param>
        /// <returns>TrainerProfileDetailResponse with trainer details</returns>
        [HttpGet("{trainerId}")]
        [ProducesResponseType(typeof(TrainerProfileDetailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTrainerProfile(string trainerId)
        {
            var clientId = GetCurrentUserId();

            var profile = await _trainerService.GetTrainerProfileAsync(trainerId);

            if (profile == null)
                return NotFound(new ApiResponse(404, "Trainer profile not found."));

            return Ok(profile);
        }
    }
}
