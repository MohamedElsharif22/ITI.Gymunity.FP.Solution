using ITI.Gymunity.FP.Application.DTOs.Program;
using ITI.Gymunity.FP.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Gymunity.FP.APIs.Areas.Trainer
{
 [Area("Trainer")]
 [Route("api/trainer/[controller]")]
 [ApiController]
 public class DayExercisesController : TrainerBaseController
 {
 private readonly IDayExerciseService _service;

 public DayExercisesController(IDayExerciseService service)
 {
 _service = service;
 }

 [HttpGet("by-day/{dayId:int}")]
 public async Task<IActionResult> GetByDay(int dayId)
 {
 var list = await _service.GetByDayAsync(dayId);
 return Ok(list);
 }

 [HttpGet("{id:int}")]
 public async Task<IActionResult> GetById(int id)
 {
 var item = await _service.GetByIdAsync(id);
 if (item == null) return NotFound();
 return Ok(item);
 }

 [HttpPost]
 public async Task<IActionResult> Create([FromBody] ProgramDayExerciseGetAllResponse request)
 {
 var created = await _service.CreateAsync(request);
 return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
 }

 [HttpPut("{id:int}")]
 public async Task<IActionResult> Update(int id, [FromBody] ProgramDayExerciseGetAllResponse request)
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
