using ITI.Gymunity.FP.APIs.Responses;
using ITI.Gymunity.FP.Infrastructure.DTOs.Trainer;
using ITI.Gymunity.FP.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Gymunity.FP.APIs.Areas.Trainer
{
    public class TrainerProfileController(TrainerProfileService trainerProfileService) : TrainerBaseController
    {
        private readonly TrainerProfileService _trainerProfileService = trainerProfileService;

        //// GET: api/trainer/trainerprofile/getallprofiles
        //[HttpGet("AllProfiles")]
        //[ProducesResponseType(typeof(IEnumerable<TrainerProfileListResponse>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> GetAllProfiles()
        //{
        //    var profiles = await _trainerProfileService.GetAllProfiles();

        //    if (!profiles.Any())
        //        return NotFound(new ApiResponse(404, "No trainer profiles found."));

        //    return Ok(profiles);
        //}

        // GET: api/trainer/trainerprofile/getbyuserid/{userId}
        [HttpGet("UserId/{userId}")]
        [Authorize] // Uncomment when authentication is ready
        [ProducesResponseType(typeof(TrainerProfileDetailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            // TODO: Add authentication check
            // var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // if (currentUserId != userId)
            //     return Unauthorized(new ApiResponse(401, "Unauthorized access."));

            var profile = await _trainerProfileService.GetProfileByUserId(userId);

            if (profile == null)
                return NotFound(new ApiResponse(404, "Trainer profile not found."));

            return Ok(profile);
        }



        // GET: api/trainer/trainerprofile/getbyid/{id}
        [HttpGet("Id/{id}")]
        [ProducesResponseType(typeof(TrainerProfileDetailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var profile = await _trainerProfileService.GetProfileById(id);

            if (profile == null)
                return NotFound(new ApiResponse(404, "Trainer profile not found."));

            return Ok(profile);
        }

        // POST: api/trainer/trainerprofile/create
        [HttpPost("")]
        // [Authorize] // Uncomment when authentication is ready
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(TrainerProfileDetailResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProfile([FromForm] CreateTrainerProfileRequest request)
        {
            try
            {
                // TODO: Add authentication check
                // var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                // if (currentUserId != request.UserId)
                //     return Unauthorized(new ApiResponse(401, "Unauthorized access."));

                var profile = await _trainerProfileService.CreateProfile(request);
                return CreatedAtAction(nameof(GetById), new { id = profile.Id }, profile);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        // PUT: api/trainer/trainerprofile/update/{id}
        [HttpPut("{id}")]
        // [Authorize] // Uncomment when authentication is ready
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(TrainerProfileDetailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProfile(int id, [FromForm] UpdateTrainerProfileRequest request)
        {
            try
            {
                // TODO: Add authentication check to ensure user owns this profile
                // var profile = await _trainerProfileService.GetProfileById(id);
                // var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                // if (profile?.UserId != currentUserId)
                //     return Unauthorized(new ApiResponse(401, "Unauthorized access."));

                var updatedProfile = await _trainerProfileService.UpdateProfile(id, request);
                return Ok(updatedProfile);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
        }

        // DELETE: api/trainer/trainerprofile/delete/{id}
        [HttpDelete("{id}")]
        // [Authorize] // Uncomment when authentication is ready
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProfile(int id)
        {
            // TODO: Add authentication check to ensure user owns this profile
            // var profile = await _trainerProfileService.GetProfileById(id);
            // var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // if (profile?.UserId != currentUserId)
            //     return Unauthorized(new ApiResponse(401, "Unauthorized access."));

            var result = await _trainerProfileService.DeleteProfile(id);

            if (!result)
                return NotFound(new ApiResponse(404, "Trainer profile not found."));

            return NoContent();
        }

        // PUT: api/trainer/trainerprofile/updatestatus/{id}
        [HttpPut("Status/{id}")]
        // [Authorize] // Uncomment when authentication is ready
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(TrainerProfileDetailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStatus(int id, [FromForm] UpdateStatusRequest request)
        {
            try
            {
                // TODO: Add authentication check to ensure user owns this profile
                // var profile = await _trainerProfileService.GetProfileById(id);
                // var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                // if (profile?.UserId != currentUserId)
                //     return Unauthorized(new ApiResponse(401, "Unauthorized access."));

                var updatedProfile = await _trainerProfileService.UpdateStatus(id, request);
                return Ok(updatedProfile);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
        }

        // Get TrainerProfile By id  

        //[HttpGet("GetById/{id:int}")]
        //[ProducesResponseType(typeof(TrainerProfileResponse), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    var profile = await _trainerProfileService.GetProfileById(id);

        //    if (profile is null)
        //        return NotFound(new ApiResponse(404, "Trainer profile not found."));

        //    return Ok(profile);
        //}

    }
}