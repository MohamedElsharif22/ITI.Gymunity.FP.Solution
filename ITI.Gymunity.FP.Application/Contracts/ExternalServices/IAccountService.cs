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
        Task<AuthResponse> ResetPasswordAsync(ResetPasswordRequest request);
        Task<AuthResponse> ChangePasswordAsync(string userId, ChangePasswordRequest request);
        Task<AuthResponse> UpdateProfileAsync(string userId, UpdateProfileRequest request);
        Task<AuthResponse> GoogleAuthAsync(GoogleAuthRequest request);
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}
