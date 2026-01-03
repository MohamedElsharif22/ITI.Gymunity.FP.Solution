using ITI.Gymunity.FP.Application.DTOs;
using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Application.Services.ClientServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITI.Gymunity.FP.APIs.Areas.Client
{
    
    public class TrainerDiscoveryController : ClientBaseController
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
        public async Task<IActionResult> SearchTrainers()
            //[FromQuery] string? search = null,
            //[FromQuery] string? specialization = null,
            //[FromQuery] int? minExperience = null,
            //[FromQuery] decimal? maxPrice = null,
            //[FromQuery] bool? isVerified = null,
            //[FromQuery] string? sortBy = "rating", // rating, experience, price, clients
            //[FromQuery] int page = 1,
            //[FromQuery] int pageSize = 20)
        {
            var clientId = GetCurrentUserId();

            var result = await _trainerService.SearchTrainersAsync();

            return Ok(result);
        }
    }
}
