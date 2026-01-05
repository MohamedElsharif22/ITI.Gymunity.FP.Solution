using ITI.Gymunity.FP.APIs.Responses;
using ITI.Gymunity.FP.APIs.Services;
using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Application.Services;
using ITI.Gymunity.FP.Domain.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITI.Gymunity.FP.APIs.Areas.Trainer
{
    public class TrainerProfileController : TrainerBaseController
    {
        private readonly TrainerProfileService _trainerProfileService;
        private readonly IAdminNotificationService _adminNotificationService;
        private readonly AdminUserResolverService _adminUserResolver;
        private readonly ILogger<TrainerProfileController> _logger;

        public TrainerProfileController(
            TrainerProfileService trainerProfileService,
            IAdminNotificationService adminNotificationService,
            AdminUserResolverService adminUserResolver,
            ILogger<TrainerProfileController> logger)
        {
            _trainerProfileService = trainerProfileService;
            _adminNotificationService = adminNotificationService;
            _adminUserResolver = adminUserResolver;
            _logger = logger;
        }

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

        /// <summary>
        /// Retrieves the profile details of the currently authenticated trainer.
        /// </summary>
        /// <remarks>This endpoint requires the user to be authenticated. The response includes detailed
        /// profile information for the trainer associated with the current user account.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing the trainer's profile details with status code 200 (OK) if found;
        /// status code 404 (Not Found) if the profile does not exist; or status code 401 (Unauthorized) if the user is
        /// not authenticated.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(TrainerProfileDetailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMyProfile()
        {
            // TODO: Add authentication check
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId is null)
                return Unauthorized(new ApiResponse(401, "Unauthorized access."));

            var profile = await _trainerProfileService.GetProfileByUserId(currentUserId);

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
        [ProducesResponseType(typeof(TrainerProfileDetailResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProfile([FromForm] CreateTrainerProfileRequest request)
        {
            try
            {
                // TODO: Add authentication check
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (currentUserId is null)
                    return Unauthorized(new ApiResponse(401, "Unauthorized access."));

                request.UserId = currentUserId;

                var profile = await _trainerProfileService.CreateProfile(request);
                
                // ✅ Notify admin of new trainer profile creation
                await NotifyAdminOfProfileCreationAsync(profile, currentUserId);
                
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

        [HttpGet("subscribers")]
        public async Task<IActionResult> GetMySubscribers() //trainer id 
        {
            var trainerId = GetTrainerId(); // من JWT

            var subscribers = await _trainerProfileService.GetSubscribersByTrainerIdAsync(trainerId);
            return Ok(subscribers);
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

        /// <summary>
        /// Sends admin notification for trainer profile creation
        /// </summary>
        private async Task NotifyAdminOfProfileCreationAsync(TrainerProfileDetailResponse profile, string userId)
        {
            try
            {
                if (profile == null)
                    return;

                var admin = await _adminUserResolver.GetPrimaryAdminAsync();
                if (admin == null)
                {
                    _logger.LogWarning("No admin user found to notify about trainer profile creation");
                    return;
                }

                await _adminNotificationService.CreateAdminNotificationAsync(
                    adminUserId: admin.Id,
                    title: "New Trainer Profile Created",
                    message: $"New trainer profile for '{profile.UserName}' (Handle: @{profile.Handle}) has been created",
                    type: NotificationType.TrainerVerificationRequired,
                    relatedEntityId: profile.Id.ToString(),
                    broadcastToAll: true
                );

                _logger.LogInformation("Admin notified of new trainer profile creation {ProfileId}", profile.Id);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to notify admin of trainer profile creation");
                // Don't rethrow - profile creation already succeeded
            }
        }
    }
}