using ITI.Gymunity.FP.APIs.Errors;
using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Gymunity.FP.APIs.Areas.Trainer
{
    public class TrainerProfileController(TrainerProfileService trainerProfileService) : TrainerBaseController
    {
        private readonly TrainerProfileService _trainerProfileService = trainerProfileService;

        [HttpGet("GetAllProfiles")]
        [ProducesResponseType(typeof(IEnumerable<TrainerProfileResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllProfiles()
        {
            var profiles = await _trainerProfileService.GetAllProfiles();

            if(!profiles.Any())
                return new ObjectResult(new ApiResponse(400, "No trainer profiles found."));

            return new OkObjectResult(profiles);
        }

    }
}
