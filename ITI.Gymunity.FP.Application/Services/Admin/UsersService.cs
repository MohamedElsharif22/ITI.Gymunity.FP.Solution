using AutoMapper;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Services.Admin
{
    /// <summary>
    /// Admin service for managing users
    /// </summary>
    public class UsersService(UserManager<AppUser> userManager, IMapper mapper)
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Get all users
        /// </summary>
        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        public async Task<AppUser?> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        /// <summary>
        /// Get users by role
        /// </summary>
        public async Task<IEnumerable<AppUser>> GetUsersByRoleAsync(string roleName)
        {
            return await _userManager.GetUsersInRoleAsync(roleName);
        }
    }
}
