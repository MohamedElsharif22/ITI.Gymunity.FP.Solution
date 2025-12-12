
using ITI.Gymunity.FP.APIs.Errors;
using ITI.Gymunity.FP.Application.DTOs.Program;
using ITI.Gymunity.FP.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Gymunity.FP.APIs.Areas.Trainer
{
    public class ExerciseLibraryController : TrainerBaseController
    {
        private readonly ExerciseLibraryService _exerciseService;

        public ExerciseLibraryController(ExerciseLibraryService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        /// <summary>
        /// Creates a new custom exercise (requires admin approval)
        /// </summary>
        [HttpPost("Create")]
        // [Authorize] // Uncomment when authentication is ready
        [ProducesResponseType(typeof(ExerciseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateExercise([FromBody] CreateExerciseRequest request)
        {
            try
            {
                // TODO: Add authentication check
                // var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                // if (currentUserId != request.TrainerId)
                //     return Unauthorized(new ApiResponse(401, "Unauthorized access."));

                var exercise = await _exerciseService.CreateExercise(request);
                return CreatedAtAction(nameof(GetExerciseById), new { id = exercise.Id }, exercise);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        /// <summary>
        /// Gets all exercises. Can filter by trainerId and approval status.
        /// </summary>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(IEnumerable<ExerciseResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllExercises(
            [FromQuery] string? trainerId = null,
            [FromQuery] bool? isApproved = null)
        {
            var exercises = await _exerciseService.GetAllExercises(trainerId, isApproved);

            if (!exercises.Any())
                return NotFound(new ApiResponse(404, "No exercises found."));

            return Ok(exercises);
        }

        /// <summary>
        /// Gets a single exercise by ID
        /// </summary>
        [HttpGet("GetById/{id}")]
        [ProducesResponseType(typeof(ExerciseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExerciseById(int id)
        {
            var exercise = await _exerciseService.GetExerciseById(id);

            if (exercise == null)
                return NotFound(new ApiResponse(404, "Exercise not found."));

            return Ok(exercise);
        }

        /// <summary>
        /// Gets all global (non-custom) exercises
        /// </summary>
        [HttpGet("Global")]
        [ProducesResponseType(typeof(IEnumerable<ExerciseResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetGlobalExercises()
        {
            var exercises = await _exerciseService.GetGlobalExercises();

            if (!exercises.Any())
                return NotFound(new ApiResponse(404, "No global exercises found."));

            return Ok(exercises);
        }

        /// <summary>
        /// Gets all exercises created by a specific trainer
        /// </summary>
        [HttpGet("MyExercises")]
        // [Authorize] // Uncomment when authentication is ready
        [ProducesResponseType(typeof(IEnumerable<ExerciseResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMyExercises([FromQuery] string trainerId)
        {
            // TODO: Get trainer ID from claims
            // var trainerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(trainerId))
                return BadRequest(new ApiResponse(400, "TrainerId is required."));

            var exercises = await _exerciseService.GetTrainerExercises(trainerId);

            if (!exercises.Any())
                return NotFound(new ApiResponse(404, "No exercises found."));

            return Ok(exercises);
        }

        /// <summary>
        /// Gets all exercises awaiting admin approval
        /// </summary>
        [HttpGet("Pending")]
        [ProducesResponseType(typeof(IEnumerable<ExerciseResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPendingApprovals()
        {
            var exercises = await _exerciseService.GetPendingApprovals();

            if (!exercises.Any())
                return NotFound(new ApiResponse(404, "No pending exercises found."));

            return Ok(exercises);
        }
    }
}