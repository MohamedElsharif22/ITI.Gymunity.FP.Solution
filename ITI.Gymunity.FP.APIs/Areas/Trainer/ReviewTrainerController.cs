using ITI.Gymunity.FP.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Gymunity.FP.APIs.Areas.Trainer
{
    [Area("Trainer")]
    [Route("api/trainer/reviews")]
    [ApiController]
    public class ReviewTrainerController : TrainerBaseController
    {
        private readonly IReviewTrainerService _service;

        public ReviewTrainerController(IReviewTrainerService service)
        {
            _service = service;
        }

        // GET: api/trainer/reviews/byTrainer/{trainerId}
        // Guest can view approved reviews for any trainer
        [HttpGet("byTrainer/{trainerId:int}")]
        public async Task<IActionResult> GetApprovedReviewsForTrainer(int trainerId)
        {
            var list = await _service.GetApprovedForTrainerAsync(trainerId);
            return Ok(list);
        }
    }
}



/*
    Standard Controller Design Approach:

    - This controller does NOT handle any authorization, identity, or role checks.
    - The controller's responsibility is limited to:
        ï Receiving HTTP requests
        ï Passing data directly to the Application Service
        ï Returning the service result as an HTTP response

    - Authentication / Authorization concerns are centralized in:
        ï Base controllers (e.g., ClientBaseController, TrainerBaseController)
        ï Global filters / middleware (JWT, Policies, Roles)

    - This ensures:
        ï Clean Controllers (Thin Controllers pattern)
        ï Single Responsibility Principle
        ï Consistent behavior across all controllers
        ï Easy switch between Anonymous / Authorized modes without rewriting controllers

    - Any future authorization logic should be applied at:
        ï Base controller level
        ï Attribute level ([Authorize], [AllowAnonymous])
        ï NOT inside action methods
*/

//Ê·Ê Õ«»» ‰”Œ… √ﬁ’—   Õÿ ›Êﬁ ﬂ· Action:

// NOTE:
// Authorization is handled at the BaseController / middleware level.
// This action assumes a valid execution context and delegates all logic to the service layer.