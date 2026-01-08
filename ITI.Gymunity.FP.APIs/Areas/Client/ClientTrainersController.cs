using ITI.Gymunity.FP.APIs.Responses;
using ITI.Gymunity.FP.Application.DTOs;
using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Application.Services.ClientServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITI.Gymunity.FP.APIs.Areas.Client
{
    
    public class ClientTrainersController(ClientTrainersService trainerService) : ClientBaseController
    {
        private readonly ClientTrainersService _trainerService = trainerService;

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("User ID not found in token");
        }

        /// <summary>
        /// Retrieves a list of trainers associated with the current user.
        /// </summary>
        /// <remarks>This method is intended for clients to obtain all trainers linked to their active
        /// subscription. The response will be empty if the user has no associated trainers.</remarks>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list of <see cref="TrainerBriefResponse"/> objects
        /// representing the user's trainers. Returns a 404 Not Found response if no trainers are found.</returns>
        [HttpGet]
        public async Task<ActionResult<List<TrainerBriefResponse>>> GetAllTrainers()
        {
            var trainers = await _trainerService.GetClientTrainers(GetCurrentUserId());

            if(!trainers.Any()) 
                return NotFound(new ApiResponse(404, "No trainers found related to your active Subscribtion."));
            
            return Ok(trainers);
        }

    }
}
