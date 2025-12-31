using ITI.Gymunity.FP.Domain.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//Fix authorization commit
namespace ITI.Gymunity.FP.APIs.Areas.Trainer
{
    [Route("api/trainer/[controller]")]
    [ApiController]
    [Authorize(Roles = "Trainer")] // Uncomment when authentication is ready

    public class TrainerBaseController : ControllerBase
    {
        protected int GetTrainerId()
        {
            // Try to read trainer id from claims (NameIdentifier expected to be trainer profile id or user id)
            var idClaim = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(idClaim, out var id)) return id;

            // If not available or not int, return 0 as invalid
            return 0;
        }
    }
}
