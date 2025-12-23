using ITI.Gymunity.FP.Application.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Contracts.ExternalServices
{
    public interface IAccountService
    {
        Task<bool> SendResetPasswordLinkAsync(ForgetPasswordRequest request);
        Task<UserResponse> ResetPasswordAsync(ResetPasswordRequest request);
        Task<UserResponse> ChangePasswordAsync(string userId, ChangePasswordRequest request);
        Task<UserResponse> UpdateProfileAsync(string userId, UpdateProfileRequest request);
        Task<UserResponse> GoogleAuthAsync(GoogleAuthRequest request);
        Task<UserResponse> RegisterAsync(RegisterRequest request);
        Task<UserResponse> LoginAsync(LoginRequest request);
    }
}
