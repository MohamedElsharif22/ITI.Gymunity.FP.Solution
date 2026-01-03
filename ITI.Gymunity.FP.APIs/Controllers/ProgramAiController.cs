using ITI.Gymunity.FP.Application.DTOs.Program;
using ITI.Gymunity.FP.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Gymunity.FP.APIs.Controllers
{
 [ApiController]
 [Route("api/trainer/[controller]")]
 public class ProgramAiController : ControllerBase
 {
 private readonly IAiProgramService _aiProgramService;
 private readonly IProgramManagerService _programManagerService;
 private readonly IProgramImportService _programImportService;
 private readonly ILogger<ProgramAiController> _logger;

 public ProgramAiController(IAiProgramService aiProgramService, IProgramManagerService programManagerService, IProgramImportService programImportService, ILogger<ProgramAiController> logger)
 {
 _aiProgramService = aiProgramService;
 _programManagerService = programManagerService;
 _programImportService = programImportService;
 _logger = logger;
 }

 [HttpPost("generate")]
 public async Task<IActionResult> GenerateProgram([FromBody] ProgramAiCreateRequest request)
 {
 if (!ModelState.IsValid) return BadRequest(ModelState);

 try
 {
 var aiResult = await _aiProgramService.GenerateProgramAsync(request);

 // Require AI to return strict JSON for automatic creation
 if (!aiResult.IsValidJson)
 {
 _logger.LogWarning("AI did not return valid JSON for trainer {TrainerProfileId}", request.TrainerProfileId);
 return BadRequest(new { success = false, error = "AI did not return valid JSON. Please refine the prompt or try again." });
 }

 // create persisted program using existing program manager
 var programReq = aiResult.Program;
 var created = await _programManagerService.CreateAsync(programReq);

 // import weeks/days/exercises into DB
 await _programImportService.ImportAiResultAsync(created.Id, aiResult);

 return Ok(new { success = true, programId = created.Id, ai = aiResult });
 }
 catch (Exception ex)
 {
 _logger.LogError(ex, "Error generating program via AI");
 return StatusCode(500, new { success = false, error = "Could not generate program" });
 }
 }
 }
}
