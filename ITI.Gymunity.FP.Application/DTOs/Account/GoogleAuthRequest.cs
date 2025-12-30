using ITI.Gymunity.FP.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.DTOs.Account
{
    public record GoogleAuthRequest
    {
        public string IdToken { get; set; } // Google ID token from frontend
        public UserRole? Role { get; set; }
    }
}
