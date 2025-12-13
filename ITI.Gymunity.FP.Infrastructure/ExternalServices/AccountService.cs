using ITI.Gymunity.FP.Application.Contracts.ExternalServices;
using ITI.Gymunity.FP.Application.DTOs.Account;
using ITI.Gymunity.FP.Application.DTOs.Account;
using ITI.Gymunity.FP.Application.Mapping;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Infrastructure.ExternalServices
{
    public class AccountService(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IFileUploadService fileUploadService,
        IAuthService authService) : IAccountService
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly IFileUploadService _fileUploadService = fileUploadService;
        private readonly IAuthService _authService = authService;

        public async Task<UserResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.EmailOrUserName)
                       ?? await _userManager.FindByNameAsync(request.EmailOrUserName);

            if (user is null)
                throw new Exception("Invalid email/username or password.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!result.Succeeded)
                throw new Exception("Invalid email/username or password.");

            var token =  await _authService.CreateTokenAsync(user, _userManager);
            return user.ToUserResponse(token);
        }

        public async Task<UserResponse> RegisterAsync(RegisterRequest request)
        {
            if (!await IsEmailUniqueAsync(request.Email))
            {
                throw new Exception("Email is already registered.");
            }
            if (!await IsHandelUniqueAsync(request.UserName))
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

            await _userManager.AddToRoleAsync(user, ((UserRole) request.Role).ToString());
            var token = await _authService.CreateTokenAsync(user, _userManager);
            return user.ToUserResponse(token);
        }

        private async Task<bool> IsHandelUniqueAsync(string handel)
        {
            var user = await _userManager.FindByNameAsync(handel);
            return user is null;
        }

        private async Task<bool> IsEmailUniqueAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user is null;
        }

        
    }
}
