using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Gymunity.FP.APIs.Areas.Admin
{
 [Route("api/admin/[controller]")]
 [ApiController]
 [AllowAnonymous] // Temporary for testing purposes
 // [Authorize(Roles = "Admin")] // Uncomment when authentication is ready
 public class AdminBaseController : ControllerBase
 {
 }
}
