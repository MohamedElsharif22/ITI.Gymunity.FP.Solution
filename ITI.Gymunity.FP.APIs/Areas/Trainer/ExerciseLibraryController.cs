using ITI.Gymunity.FP.Application.DTOs.ExerciseLibrary;
using ITI.Gymunity.FP.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Gymunity.FP.APIs.Areas.Trainer
{
 [Area("Trainer")]
 [Route("api/trainer/[controller]")]
 [ApiController]
 public class ExerciseLibraryController : TrainerBaseController
 {
 private readonly IExerciseLibraryService _service;

 public ExerciseLibraryController(IExerciseLibraryService service)
 {
 _service = service;
 }

 [HttpGet]
 public async Task<IActionResult> GetAll([FromQuery] string? trainerId = null)
 {
 var list = await _service.GetAllAsync(trainerId);
 return Ok(list);
 }

 [HttpGet("{id:int}")]
 public async Task<IActionResult> GetById(int id)
 {
 var item = await _service.GetByIdAsync(id);
 if (item == null) return NotFound();
 return Ok(item);
 }

 [HttpGet("search")]
 public async Task<IActionResult> Search([FromQuery] string name, [FromQuery] string? trainerId = null)
 {
 var list = await _service.SearchByNameAsync(name, trainerId);
 return Ok(list);
 }

 [HttpPost]
 public async Task<IActionResult> Create([FromBody] ExerciseCreateRequest request)
 {
 var created = await _service.CreateAsync(request);
 return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
 }

 [HttpPut("{id:int}")]
 public async Task<IActionResult> Update(int id, [FromBody] ExerciseUpdateRequest request)
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
 }
}
