using ITI.Gymunity.FP.Application.DTOs.Account;
using ITI.Gymunity.FP.Application.Services;
using ITI.Gymunity.FP.Domain.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Gymunity.FP.APIs.Controllers
{
    public class AccountController(AccountService accountService) : BaseApiController
    {
        private readonly AccountService _accountService = accountService;

        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> Register([FromForm] RegisterRequest request)
        {
            try
            {
                var userResponse = await _accountService.RegisterUserAsync(request);
                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



    }
}
