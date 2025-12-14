using ITI.Gymunity.FP.Application.DTOs.Program;
using ITI.Gymunity.FP.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Gymunity.FP.APIs.Areas.Trainer
{
 [Area("Trainer")]
 [Route("api/trainer/[controller]")]
 [ApiController]
 public class ProgramsController : TrainerBaseController
 {
 private readonly IProgramManagerService _service;

 public ProgramsController(IProgramManagerService service)
 {
 _service = service;
 }

 [HttpGet]
 public async Task<IActionResult> GetAll()
 {
 var list = await _service.GetAllAsync();
 return Ok(list);
 }

 [HttpGet("search")]
 public async Task<IActionResult> Search([FromQuery] string? term)
 {
 var list = await _service.SearchAsync(term);
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
 public async Task<IActionResult> Create([FromBody] ProgramCreateRequest request)
 {
 var created = await _service.CreateAsync(request);
 return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
 }

 [HttpPut("{id:int}")]
 public async Task<IActionResult> Update(int id, [FromBody] ProgramUpdateRequest request)
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
