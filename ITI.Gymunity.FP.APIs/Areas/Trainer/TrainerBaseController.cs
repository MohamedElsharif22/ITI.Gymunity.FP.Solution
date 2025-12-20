using ITI.Gymunity.FP.Domain.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//Fix authorization commit
namespace ITI.Gymunity.FP.APIs.Areas.Trainer
{
    [Route("api/trainer/[controller]")]
    [ApiController]
    [AllowAnonymous] // Temporary for testing purposes
    //[Authorize(Roles = "Trainer")] // Uncomment when authentication is ready

    public class TrainerBaseController : ControllerBase
    {
    }
}
