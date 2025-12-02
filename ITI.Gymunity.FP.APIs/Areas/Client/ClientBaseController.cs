using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Gymunity.FP.APIs.Areas.Client
{
    [Route("api/client/[controller]")]
    [ApiController]
    [Authorize(Roles = "Client")]
    public class ClientBaseController : ControllerBase
    {
    }
}
