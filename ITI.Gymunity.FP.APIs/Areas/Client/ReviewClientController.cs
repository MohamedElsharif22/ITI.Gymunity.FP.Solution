using ITI.Gymunity.FP.Application.Services;
using ITI.Gymunity.FP.Application.DTOs.Trainer;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Gymunity.FP.APIs.Areas.Client
{
    public class ReviewClientController : ClientBaseController
    {
        private readonly IReviewClientService _service;

        public ReviewClientController(IReviewClientService service)
        {
            _service = service;
        }

        [HttpPost("trainer/{trainerId:int}")]
        public async Task<IActionResult> CreateForTrainer(
            int trainerId,
            [FromBody] TrainerReviewCreateRequest request)
        {
            var created = await _service.CreateAsync(
                CurrentClientId,
                trainerId,
                request
            );

            return Ok(created);
        }
    }
}
