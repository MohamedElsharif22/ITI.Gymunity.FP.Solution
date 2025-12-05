using ITI.Gymunity.FP.Domain.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Gymunity.FP.APIs.Areas.Trainer
{
    [Route("api/trainer/[controller]")]
    [ApiController]
    // [Authorize(Roles = "Trainer")] // Uncomment when authentication is ready
    [AllowAnonymous] // Remove this when authentication is ready
    public class TrainerBaseController : ControllerBase
    {
    }
}