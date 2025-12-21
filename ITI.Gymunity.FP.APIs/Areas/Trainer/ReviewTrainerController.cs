using ITI.Gymunity.FP.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Gymunity.FP.APIs.Areas.Trainer
{
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
        � Receiving HTTP requests
        � Passing data directly to the Application Service
        � Returning the service result as an HTTP response

    - Authentication / Authorization concerns are centralized in:
        � Base controllers (e.g., ClientBaseController, TrainerBaseController)
        � Global filters / middleware (JWT, Policies, Roles)

    - This ensures:
        � Clean Controllers (Thin Controllers pattern)
        � Single Responsibility Principle
        � Consistent behavior across all controllers
        � Easy switch between Anonymous / Authorized modes without rewriting controllers

    - Any future authorization logic should be applied at:
        � Base controller level
        � Attribute level ([Authorize], [AllowAnonymous])
        � NOT inside action methods
*/

//��� ���� ���� ���� ���� ��� �� Action:

// NOTE:
// Authorization is handled at the BaseController / middleware level.
// This action assumes a valid execution context and delegates all logic to the service layer.