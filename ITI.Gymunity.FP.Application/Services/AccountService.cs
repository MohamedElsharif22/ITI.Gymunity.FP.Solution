using ITI.Gymunity.FP.Application.Contracts.ExternalServices;
using ITI.Gymunity.FP.Application.DTOs.Account;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Services
{
    public class AccountService(UserManager<AppUser> userManager, 
        IFileUploadService fileUploadService,
        IAuthService authService)
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IFileUploadService _fileUploadService = fileUploadService;
        private readonly IAuthService _authService = authService;

        public async Task<UserResponse> RegisterUserAsync(RegisterRequest request)
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
            return new UserResponse
            {
                Name = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role.ToString(),
                Token = token
            };
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
