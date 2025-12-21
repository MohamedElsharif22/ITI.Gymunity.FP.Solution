using ITI.Gymunity.FP.Infrastructure.Contracts.ExternalServices;
using ITI.Gymunity.FP.Infrastructure.DTOs.Account;
using ITI.Gymunity.FP.Infrastructure.DTOs.Email;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITI.Gymunity.FP.Application.Mapping;

namespace ITI.Gymunity.FP.Infrastructure.ExternalServices
{
    public class AccountService(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IFileUploadService fileUploadService,
        IAuthService authService,
        IGoogleAuthService googleAuthService,
        IEmailService emailService,
        ILogger<AccountService> logger,
        IConfiguration configuration) : IAccountService
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly IFileUploadService _fileUploadService = fileUploadService;
        private readonly IAuthService _authService = authService;
        private readonly IGoogleAuthService _googleAuthService = googleAuthService;
        private readonly IEmailService _emailService = emailService;
        private readonly ILogger<AccountService> _logger = logger;
        private readonly IConfiguration _configuration = configuration;

        public async Task<UserResponse> GoogleAuthAsync(GoogleAuthRequest request)
        {
            // Validate Google token
            var googleUser = await _googleAuthService.ValidateGoogleTokenAsync(request.IdToken)
                ?? throw new Exception("Invalid Google token");

            if (!googleUser.EmailVerified)
                throw new Exception("Google email is not verified");


            // Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(googleUser.Email);

            AppUser user;

            if (existingUser != null)
            {
                // User exists - Login
                user = existingUser;

                // Check if Google login is already linked
                var logins = await _userManager.GetLoginsAsync(user);
                var googleLogin = logins.FirstOrDefault(l => l.LoginProvider == "Google");

                if (googleLogin == null)
                {
                    // Link Google account to existing user
                    var addLoginResult = await _userManager.AddLoginAsync(user,
                        new UserLoginInfo("Google", googleUser.GoogleId, "Google"));

                    if (!addLoginResult.Succeeded)
                    {
                        _logger.LogError("Failed to link Google account for user {Email}", googleUser.Email);
                    }
                }
            }
            else
            {
                // User doesn't exist - Register
                user = new AppUser
                {
                    FullName = $"{googleUser.FirstName} {googleUser.LastName}",
                    Email = googleUser.Email,
                    UserName = googleUser.Email,
                    EmailConfirmed = true // Google email is already verified
                };

                // Create user without password (external login)
                var createResult = await _userManager.CreateAsync(user);

                if (!createResult.Succeeded)
                {
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create user: {errors}");
                }


                // Add external login info
                var addLoginResult = await _userManager.AddLoginAsync(user,
                    new UserLoginInfo("Google", googleUser.GoogleId, "Google"));

                if (!addLoginResult.Succeeded)
                {
                    _logger.LogError("Failed to add Google login for new user {Email}", googleUser.Email);
                }

                var roleResult = await _userManager.AddToRoleAsync(user, (UserRole.Client).ToString());
                // Assign default role
                if (!roleResult.Succeeded)
                {
                    _logger.LogError("Failed to assign role to user {Email}", googleUser.Email);
                }
            }

            // Generate JWT token
            var token = await _authService.CreateTokenAsync(user, _userManager);

            var emailRequset = new EmailRequest()
            {
                ToEmail = user.Email,
                ToName = user.UserName ?? "",
                Subject = "Login Success",
                Body = "You succesfully Signed in to Gymunity!"
            };

            await _emailService.SendEmailAsync(emailRequset);

            return user.ToUserResponse(token);
        }


        public async Task<UserResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.EmailOrUserName)
                       ?? await _userManager.FindByNameAsync(request.EmailOrUserName);

            if (user is null)
                throw new Exception("Invalid email/username or password.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!result.Succeeded)
                throw new Exception("Invalid email/username or password.");

            var token = await _authService.CreateTokenAsync(user, _userManager);

            var emailRequset = new EmailRequest()
            {
                ToEmail = user.Email!,
                ToName = user.UserName ?? "",
                Subject = "Login Success",
                Body = "You succesfully Signed in to Gymunity!"
            };
            await _emailService.SendEmailAsync(emailRequset);
            return user.ToUserResponse(token);
        }

        public async Task<UserResponse> RegisterAsync(RegisterRequest request)
        {
            if (!await IsEmailUniqueAsync(request.Email))
            {

                throw new Exception("Email is already registered.");
            }
            if (!await IsUserNameUniqueAsync(request.UserName))
            {
                throw new Exception("Handle is already taken.");
            }
            var user = new AppUser
            {
                UserName = request.UserName.ToLower(),
                Email = request.Email,
                FullName = request.FullName,
                Role = (UserRole)request.Role,
            };
            if (request.ProfilePhoto is not null)
            {
                if (!_fileUploadService.IsValidImageFile(request.ProfilePhoto))
                {
                    throw new Exception("Invalid profile photo format.");
                }
                var photoPath = await _fileUploadService.UploadImageAsync(request.ProfilePhoto, IFileUploadService.UserProfilePhotosFolder);
                user.ProfilePhotoUrl = photoPath;
            }
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _fileUploadService.DeleteImage(user.ProfilePhotoUrl ?? string.Empty);
                throw new Exception($"User registration failed: {errors}");
            }

            await _userManager.AddToRoleAsync(user, ((UserRole)request.Role).ToString());
            var token = await _authService.CreateTokenAsync(user, _userManager);
            var emailRequset = new EmailRequest()
            {
                ToEmail = user.Email!,
                ToName = user.UserName ?? "",
                Subject = "Registration Success",
                Body = "You succesfully Registered to Gymunity!"
            };

            await _emailService.SendEmailAsync(emailRequset);
            return user.ToUserResponse(token);
        }

        private async Task<bool> IsUserNameUniqueAsync(string handel)
        {
            var user = await _userManager.FindByNameAsync(handel);
            return user is null;
        }

        private async Task<bool> IsEmailUniqueAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user is null;
        }

        public async Task<UserResponse> UpdateProfileAsync(string? userId, UpdateProfileRequest request)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("UpdateProfileAsync called with null or empty userId");
                throw new Exception("Invalid Operation.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                _logger.LogWarning("User not found for UpdateProfileAsync with userId: {UserId}", userId);
                throw new Exception("Invalid Operation.");
            }

            if (!string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase))
            {
                if (!await IsEmailUniqueAsync(request.Email))
                {
                    throw new Exception("Email is already registered.");
                }
                user.Email = request.Email;
            }

            if (!string.Equals(user.UserName, request.UserName, StringComparison.OrdinalIgnoreCase))
            {
                if (!await IsUserNameUniqueAsync(request.UserName))
                {
                    throw new Exception("Handle is already taken.");
                }
                user.UserName = request.UserName;
            }

            user.FullName = request.FullName;

            if (request.ProfilePhoto is not null)
            {
                if (!_fileUploadService.IsValidImageFile(request.ProfilePhoto))
                {
                    throw new Exception("Invalid profile photo format.");
                }
                // Delete old photo if exists
                var photoPath = await _fileUploadService.UploadImageAsync(request.ProfilePhoto, IFileUploadService.UserProfilePhotosFolder);
                if (!string.IsNullOrEmpty(photoPath))
                {
                    var isDeleted = user.ProfilePhotoUrl is not null ? _fileUploadService.DeleteImage(user.ProfilePhotoUrl) : false;
                }
                user.ProfilePhotoUrl = photoPath;
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Profile update failed: {errors}");
            }

            var token = await _authService.CreateTokenAsync(user, _userManager);

            var emailRequset = new EmailRequest()
            {
                ToEmail = user.Email!,
                ToName = user.UserName ?? "",
                Subject = "Profile Update Success",
                Body = "You succesfully Updated your profile in Gymunity!"
            };
            await _emailService.SendEmailAsync(emailRequset);
            return user.ToUserResponse(token);
        }

        public async Task<UserResponse> ChangePasswordAsync(string userId, ChangePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new Exception("Invalid Operation.");

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Password update failed: {errors}");
            }
            var token = await _authService.CreateTokenAsync(user, _userManager);

            var emailRequset = new EmailRequest()
            {
                ToEmail = user.Email!,
                ToName = user.UserName ?? "",
                Subject = "Password Change Success",
                Body = "You succesfully Changed your password in Gymunity!"
            };

            return user.ToUserResponse(token);
        }

        public async Task<bool> SendResetPasswordLinkAsync(ForgetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                _logger.LogWarning("ForgetPassword requested for non-existing email: {Email}", request.Email);
                return false;
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var frontendUrl = _configuration["FrontendOrigins:Local"] ?? _configuration["FrontendOrigins:Hosted"];
            var resetLink = $"{frontendUrl}/reset-password?email={Uri.EscapeDataString(user.Email!)}&token={Uri.EscapeDataString(token)}";
            var emailRequest = new EmailRequest
            {
                ToEmail = user.Email!,
                ToName = user.FullName ?? "",
                Subject = "Password Reset Request",
                Body = $"Click the link to reset your password: {resetLink}"
            };
            await _emailService.SendEmailAsync(emailRequest);
            return true;
        }

        public async Task<UserResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email)
                ?? throw new Exception("Invalid email address.");
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Password reset failed: {errors}");
            }
            var token = await _authService.CreateTokenAsync(user, _userManager);
            var emailRequset = new EmailRequest()
            {
                ToEmail = user.Email!,
                ToName = user.UserName ?? "",
                Subject = "Password Reset Success",
                Body = "You succesfully Reset your password in Gymunity!"
            };
            await _emailService.SendEmailAsync(emailRequset);
            return user.ToUserResponse(token);
        }
    }
}
