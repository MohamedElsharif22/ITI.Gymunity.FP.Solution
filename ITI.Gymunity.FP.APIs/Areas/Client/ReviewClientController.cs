using ITI.Gymunity.FP.Application.Services;
using ITI.Gymunity.FP.Application.DTOs.Trainer;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITI.Gymunity.FP.APIs.Areas.Client
{
    public class ReviewClientController : ClientBaseController
    {
        private readonly IReviewClientService _service;

        public ReviewClientController(IReviewClientService service)
        {
            _service = service;
        }

        private string GetUserId() {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        [HttpPost("trainer/{trainerId:int}")]
        public async Task<IActionResult> CreateForTrainer(
            int trainerId,
            [FromBody] TrainerReviewCreateRequest request)
        {
            try
            {
                var created = await _service.CreateAsync(
                    GetUserId(),
                    trainerId,
                    request
                );

                return Ok(created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("{reviewId:int}")]
        public async Task<IActionResult> Update(int reviewId, [FromBody] TrainerReviewCreateRequest request)
        {
            var updated = await _service.UpdateAsync(GetUserId(), reviewId, request);
            if (updated == null) return BadRequest(new { success = false, message = "Update failed or review not found/owned by user" });
            return Ok(updated);
        }

        [HttpDelete("{reviewId:int}")]
        public async Task<IActionResult> Delete(int reviewId)
        {
            var deleted = await _service.DeleteAsync(GetUserId(), reviewId);
            if (!deleted) return BadRequest(new { success = false, message = "Delete failed or review not found/owned by user" });
            return Ok(new { success = true });
        }
    }
}
