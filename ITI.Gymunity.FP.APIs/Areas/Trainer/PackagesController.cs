using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Gymunity.FP.APIs.Areas.Trainer
{
 [Area("Trainer")]
 [Route("api/trainer/Packages")]
 [ApiController]
 [Authorize]
 public class PackagesController : TrainerBaseController
 {
 private readonly IPackageService _service;

 public PackagesController(IPackageService service)
 {
 _service = service;
 }

 [HttpGet]
 public async Task<IActionResult> GetAllForTrainer()
 {
 // trainerId from current user
 var trainerId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
 if (string.IsNullOrEmpty(trainerId)) return Unauthorized();
 var list = await _service.GetAllForTrainerAsync(trainerId);
 return Ok(list);
 }

 [HttpGet("{id:int}")]
 public async Task<IActionResult> GetById(int id)
 {
 var p = await _service.GetByIdAsync(id);
 if (p == null) return NotFound();
 return Ok(p);
 }

 [HttpPost]
 public async Task<IActionResult> Create([FromBody] PackageCreateRequest request)
 {
 var trainerId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
 if (string.IsNullOrEmpty(trainerId)) return Unauthorized();
 var created = await _service.CreateAsync(trainerId, request);
 return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
 }

 [HttpPut("{id:int}")]
 public async Task<IActionResult> Update(int id, [FromBody] PackageCreateRequest request)
 {
 var ok = await _service.UpdateAsync(id, request);
 if (!ok) return NotFound();
 return NoContent();
 }

 [HttpDelete("{id:int}")]
 public async Task<IActionResult> Delete(int id)
 {
 var ok = await _service.DeleteAsync(id);
 if (!ok) return NotFound();
 return NoContent();
 }

 [HttpPatch("{id:int}/toggle-active")]
 public async Task<IActionResult> ToggleActive(int id)
 {
 var ok = await _service.ToggleActiveAsync(id);
 if (!ok) return NotFound();
 return NoContent();
 }
 }
}
