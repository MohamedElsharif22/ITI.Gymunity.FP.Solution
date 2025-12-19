using ITI.Gymunity.FP.Application.Services;
using Microsoft.AspNetCore.Mvc;
using ITI.Gymunity.FP.Application.DTOs.Admin;

namespace ITI.Gymunity.FP.APIs.Areas.Admin
{
 public class ReviewAdminController : AdminBaseController
 {
 private readonly IReviewAdminService _service;
 public ReviewAdminController(IReviewAdminService service)
 {
 _service = service;
 }

 [HttpGet("pending")]
 public async Task<IActionResult> GetPending()
 {
 var list = await _service.GetAllPendingAsync();
 return Ok(list);
 }

 [HttpPost("{id:int}/approve")]
 public async Task<IActionResult> Approve(int id)
 {
 var res = await _service.ApproveAsync(id);
 if (res == null) return NotFound();
 return Ok(res);
 }

 [HttpPost("{id:int}/reject")]
 public async Task<IActionResult> Reject(int id)
 {
 var res = await _service.RejectAsync(id);
 if (res == null) return NotFound();
 return Ok(res);
 }

 [HttpDelete("{id:int}")]
 public async Task<IActionResult> Delete(int id)
 {
 var ok = await _service.DeletePermanentAsync(id);
 if (!ok) return NotFound();
 return NoContent();
 }
 }
}
