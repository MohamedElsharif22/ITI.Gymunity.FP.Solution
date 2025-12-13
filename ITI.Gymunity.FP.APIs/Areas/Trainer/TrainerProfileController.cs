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

            if (!profiles.Any())
                return new ObjectResult(new ApiResponse(400, "No trainer profiles found."));

            return new OkObjectResult(profiles);
        }

        // Get TrainerProfile By id  

        [HttpGet("GetById/{id:int}")]
        [ProducesResponseType(typeof(TrainerProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var profile = await _trainerProfileService.GetProfileById(id);

            if (profile is null)
                return NotFound(new ApiResponse(404, "Trainer profile not found."));

            return Ok(profile);
        }

        [HttpPost("Create")]
        [ProducesResponseType(typeof(TrainerProfileResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateTrainerProfileRequest request)
        {
            try
            {
                var created = await _trainerProfileService.CreateProfileAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = created!.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }


    }
}
