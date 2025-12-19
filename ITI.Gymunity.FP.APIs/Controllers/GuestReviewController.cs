using ITI.Gymunity.FP.Application.Services;
using Microsoft.AspNetCore.Mvc;
using ITI.Gymunity.FP.Application.DTOs.Guest;

namespace ITI.Gymunity.FP.APIs.Controllers
{
 [Route("api/guest/reviews")]
 [ApiController]
 public class GuestReviewController : ControllerBase
 {
 private readonly IGuestReviewService _service;
 public GuestReviewController(IGuestReviewService service)
 {
 _service = service;
 }

 [HttpGet("byTrainer/{profileId:int}")]
 public async Task<IActionResult> GetByTrainer(int profileId)
 {
 var res = await _service.GetApprovedReviewsByTrainerAsync(profileId);
 return Ok(res);
 }

 [HttpGet("top-trainers")]
 public async Task<IActionResult> GetTopTrainers()
 {
 var res = await _service.GetTopTrainersAsync();
 return Ok(res);
 }
 }
}
