using ITI.Gymunity.FP.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Services.Admin
{
    public class UsersService(UserManager<AppUser> userManager)
    {
        private readonly UserManager<AppUser> userManager = userManager;

        public async Task<IList<AppUser>> GetAllUsersAsync()
        {
            return userManager.Users.ToList();
        }
    }
}
