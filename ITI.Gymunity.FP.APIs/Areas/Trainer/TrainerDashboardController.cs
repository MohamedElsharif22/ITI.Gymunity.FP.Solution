// ============================================
// TrainerDashboardController.cs (COMPLETE)
// Location: APIs/Areas/Trainer/
// ============================================
using ITI.Gymunity.FP.APIs.Errors;
using ITI.Gymunity.FP.Application.DTOs.Program;
using ITI.Gymunity.FP.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Gymunity.FP.APIs.Areas.Trainer
{
    public class TrainerDashboardController : TrainerBaseController
    {
        private readonly ProgramService _programService;
        private readonly ProgramWeekService _weekService;
        private readonly ProgramDayService _dayService;
        private readonly ProgramDayExerciseService _dayExerciseService;

        public TrainerDashboardController(
            ProgramService programService,
            ProgramWeekService weekService,
            ProgramDayService dayService,
            ProgramDayExerciseService dayExerciseService)
        {
            _programService = programService;
            _weekService = weekService;
            _dayService = dayService;
            _dayExerciseService = dayExerciseService;
        }

        // ==================== PROGRAM ENDPOINTS ====================

        /// <summary>
        /// Creates a new program with Week 1 and 7 days automatically
        /// </summary>
        [HttpPost("Program/Create")]
        // [Authorize] // Uncomment when authentication is ready
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ProgramResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProgram([FromForm] CreateProgramRequest request)
        {
            try
            {
                // TODO: Add authentication check
                // var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                // if (currentUserId != request.TrainerId)
                //     return Unauthorized(new ApiResponse(401, "Unauthorized access."));

                var program = await _programService.CreateProgram(request);
                return CreatedAtAction(nameof(GetProgramById), new { id = program.Id }, program);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        /// <summary>
        /// Updates an existing program. Can upload new image or remove existing one.
        /// </summary>
        [HttpPut("Program/Update/{id}")]
        // [Authorize] // Uncomment when authentication is ready
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ProgramResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProgram(int id, [FromForm] UpdateProgramRequest request)
        {
            try
            {
                // TODO: Add authentication check to ensure user owns this program
                var program = await _programService.UpdateProgram(id, request);
                return Ok(program);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
        }

        /// <summary>
        /// Soft deletes a program. Cannot delete if program has active subscriptions.
        /// </summary>
        [HttpDelete("Program/Delete/{id}")]
        // [Authorize] // Uncomment when authentication is ready
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteProgram(int id)
        {
            try
            {
                // TODO: Add authentication check to ensure user owns this program
                var result = await _programService.DeleteProgram(id);

                if (!result)
                    return NotFound(new ApiResponse(404, "Program not found."));

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        /// <summary>
        /// Gets all programs. Can filter by trainerId.
        /// </summary>
        [HttpGet("Program/GetAll")]
        [ProducesResponseType(typeof(IEnumerable<ProgramResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllPrograms([FromQuery] string? trainerId = null)
        {
            var programs = await _programService.GetAllPrograms(trainerId);

            if (!programs.Any())
                return NotFound(new ApiResponse(404, "No programs found."));

            return Ok(programs);
        }

        /// <summary>
        /// Gets a single program by ID
        /// </summary>
        [HttpGet("Program/GetById/{id}")]
        [ProducesResponseType(typeof(ProgramResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProgramById(int id)
        {
            var program = await _programService.GetProgramById(id);

            if (program == null)
                return NotFound(new ApiResponse(404, "Program not found."));

            return Ok(program);
        }

        /// <summary>
        /// Gets program with all nested weeks and days
        /// </summary>
        [HttpGet("Program/GetDetail/{id}")]
        [ProducesResponseType(typeof(ProgramDetailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProgramDetail(int id)
        {
            var program = await _programService.GetProgramDetailById(id);

            if (program == null)
                return NotFound(new ApiResponse(404, "Program not found."));

            return Ok(program);
        }

        // ==================== WEEK ENDPOINTS ====================

        /// <summary>
        /// Adds a new week to a program. Automatically creates 7 days.
        /// </summary>
        [HttpPost("Week/Add")]
        // [Authorize] // Uncomment when authentication is ready
        [ProducesResponseType(typeof(ProgramWeekResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddWeek([FromBody] AddWeekRequest request)
        {
            try
            {
                var week = await _weekService.AddWeek(request);
                return CreatedAtAction(nameof(GetWeekById), new { id = week.Id }, week);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        /// <summary>
        /// Deletes a week and all its 7 days (soft delete)
        /// </summary>
        [HttpDelete("Week/Delete/{id}")]
        // [Authorize] // Uncomment when authentication is ready
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteWeek(int id)
        {
            var result = await _weekService.DeleteWeek(id);

            if (!result)
                return NotFound(new ApiResponse(404, "Week not found."));

            return NoContent();
        }

        /// <summary>
        /// Gets all weeks. Can filter by programId.
        /// </summary>
        [HttpGet("Week/GetAll")]
        [ProducesResponseType(typeof(IEnumerable<ProgramWeekResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllWeeks([FromQuery] int? programId = null)
        {
            var weeks = await _weekService.GetAllWeeks(programId);

            if (!weeks.Any())
                return NotFound(new ApiResponse(404, "No weeks found."));

            return Ok(weeks);
        }

        /// <summary>
        /// Gets a single week by ID
        /// </summary>
        [HttpGet("Week/GetById/{id}")]
        [ProducesResponseType(typeof(ProgramWeekResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWeekById(int id)
        {
            var week = await _weekService.GetWeekById(id);

            if (week == null)
                return NotFound(new ApiResponse(404, "Week not found."));

            return Ok(week);
        }

        /// <summary>
        /// Gets week with all nested days
        /// </summary>
        [HttpGet("Week/GetDetail/{id}")]
        [ProducesResponseType(typeof(ProgramWeekDetailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWeekDetail(int id)
        {
            var week = await _weekService.GetWeekDetailById(id);

            if (week == null)
                return NotFound(new ApiResponse(404, "Week not found."));

            return Ok(week);
        }

        // ==================== DAY ENDPOINTS ====================

        /// <summary>
        /// Gets all days. Can filter by weekId.
        /// </summary>
        [HttpGet("Day/GetAll")]
        [ProducesResponseType(typeof(IEnumerable<ProgramDayResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllDays([FromQuery] int? weekId = null)
        {
            var days = await _dayService.GetAllDays(weekId);

            if (!days.Any())
                return NotFound(new ApiResponse(404, "No days found."));

            return Ok(days);
        }

        /// <summary>
        /// Gets a single day by ID
        /// </summary>
        [HttpGet("Day/GetById/{id}")]
        [ProducesResponseType(typeof(ProgramDayResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDayById(int id)
        {
            var day = await _dayService.GetDayById(id);

            if (day == null)
                return NotFound(new ApiResponse(404, "Day not found."));

            return Ok(day);
        }

        /// <summary>
        /// Gets day with all nested exercises
        /// </summary>
        [HttpGet("Day/GetDetail/{id}")]
        [ProducesResponseType(typeof(ProgramDayDetailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDayDetail(int id)
        {
            var day = await _dayService.GetDayDetailById(id);

            if (day == null)
                return NotFound(new ApiResponse(404, "Day not found."));

            return Ok(day);
        }

        // ==================== DAY EXERCISE ENDPOINTS ====================

        /// <summary>
        /// Adds an exercise to a day with all its details
        /// </summary>
        [HttpPost("DayExercise/Add")]
        // [Authorize] // Uncomment when authentication is ready
        [ProducesResponseType(typeof(ProgramDayExerciseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddExerciseToDay([FromBody] AddExerciseToDayRequest request)
        {
            try
            {
                var exercise = await _dayExerciseService.AddExercise(request);
                return CreatedAtAction(nameof(GetDayExerciseById), new { id = exercise.Id }, exercise);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        /// <summary>
        /// Updates exercise details in a day
        /// </summary>
        [HttpPut("DayExercise/Update/{id}")]
        // [Authorize] // Uncomment when authentication is ready
        [ProducesResponseType(typeof(ProgramDayExerciseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDayExercise(int id, [FromBody] UpdateExerciseInDayRequest request)
        {
            try
            {
                var exercise = await _dayExerciseService.UpdateExercise(id, request);
                return Ok(exercise);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
        }

        /// <summary>
        /// Removes an exercise from a day (soft delete)
        /// </summary>
        [HttpDelete("DayExercise/Delete/{id}")]
        // [Authorize] // Uncomment when authentication is ready
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDayExercise(int id)
        {
            var result = await _dayExerciseService.DeleteExercise(id);

            if (!result)
                return NotFound(new ApiResponse(404, "Exercise not found."));

            return NoContent();
        }

        /// <summary>
        /// Gets all exercises. Can filter by dayId.
        /// </summary>
        [HttpGet("DayExercise/GetAll")]
        [ProducesResponseType(typeof(IEnumerable<ProgramDayExerciseResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllDayExercises([FromQuery] int? dayId = null)
        {
            var exercises = await _dayExerciseService.GetAllExercises(dayId);

            if (!exercises.Any())
                return NotFound(new ApiResponse(404, "No exercises found."));

            return Ok(exercises);
        }

        /// <summary>
        /// Gets a single exercise by ID
        /// </summary>
        [HttpGet("DayExercise/GetById/{id}")]
        [ProducesResponseType(typeof(ProgramDayExerciseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDayExerciseById(int id)
        {
            var exercise = await _dayExerciseService.GetExerciseById(id);

            if (exercise == null)
                return NotFound(new ApiResponse(404, "Exercise not found."));

            return Ok(exercise);
        }
    }
}

