using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Gymunity.FP.APIs.Areas.Client
{
    [Route("api/client/[controller]")]
    [ApiController]
    [AllowAnonymous] // Temporary for testing purposes
    // [Authorize(Roles = "Client")] // Uncomment when authentication is ready
    public class ClientBaseController : ControllerBase
    {
        protected string CurrentClientId =>
        User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;

    }
}
