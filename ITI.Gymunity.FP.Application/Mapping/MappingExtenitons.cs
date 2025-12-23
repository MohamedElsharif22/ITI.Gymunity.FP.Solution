using ITI.Gymunity.FP.Application.DTOs.Account;
using ITI.Gymunity.FP.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Mapping
{
    public static class MappingExtenitons
    {
        public static UserResponse ToUserResponse(this AppUser user, string token, string profilePhotoUrl)
        {
            return new UserResponse
            {
                //Id = user.Id, // amr edit
                Name = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role.ToString(),
                ProfilePhotoUrl = profilePhotoUrl,
                Token = token,
            };
        }
    }
}
