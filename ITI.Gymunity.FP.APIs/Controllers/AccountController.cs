using ITI.Gymunity.FP.APIs.Errors;
using ITI.Gymunity.FP.Application.Contracts.ExternalServices;
using ITI.Gymunity.FP.Application.DTOs.Account;
using ITI.Gymunity.FP.Application.DTOs.Account;
using ITI.Gymunity.FP.Domain.Models.Identity;
using ITI.Gymunity.FP.Infrastructure.ExternalServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Gymunity.FP.APIs.Controllers
{
    public class AccountController(IAccountService accountService) : BaseApiController
    {
        private readonly IAccountService _accountService = accountService;

        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> Register([FromForm] RegisterRequest request)
        {
            try
            {
                var userResponse = await _accountService.RegisterAsync(request);
                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest,ex.Message));
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserResponse>> Login([FromBody] LoginRequest request)
        {
            try
            {
                var userResponse = await _accountService.LoginAsync(request);
                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, ex.Message));
            }
        }


    }
}
