using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Infrastructure.DTOs.Account
{
    public class UserResponse
    {
        public string Name { get; set; } = null!;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
