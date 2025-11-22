using ITI.Gymunity.FP.APIs.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Gymunity.FP.APIs.Controllers
{
    [Route("/errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        public ActionResult Error(int code)
        {
            return code switch
            {
                400 => BadRequest(new ApiResponse(400)),
                401 => Unauthorized(new ApiResponse(401)),
                403 => new ObjectResult(new ApiResponse(403)) { StatusCode = 403 },
                404 => NotFound(new ApiResponse(404)),
                500 => new ObjectResult(new ApiExceptionResponse(500, "Internal Server Error", "An error occurred. Please try again later.")) { StatusCode = 500 },
                _ => new ObjectResult(new ApiResponse(code)) { StatusCode = code }
            };
        }
    }
}
