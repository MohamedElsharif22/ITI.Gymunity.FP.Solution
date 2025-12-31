using ITI.Gymunity.FP.Domain.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

//Fix authorization commit
namespace ITI.Gymunity.FP.APIs.Areas.Trainer
{
    [Route("api/trainer/[controller]")]
    [ApiController]
    [Authorize(Roles = "Trainer")] // Uncomment when authentication is ready

    public class TrainerBaseController : ControllerBase
    {
        protected string GetTrainerId()
        {
            // Try to read trainer id from claims (NameIdentifier expected to be trainer profile id or user id)
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
          
        }
    }
}
