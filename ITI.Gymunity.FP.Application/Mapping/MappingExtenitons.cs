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
        public static AuthResponse ToUserResponse(this AppUser user, string token, string profilePhotoUrl)
        {
            return new AuthResponse
            {
                Id = user.Id, //amr edit i need it to test some end points
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
