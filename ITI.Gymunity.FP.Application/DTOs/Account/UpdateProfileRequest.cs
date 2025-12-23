using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.DTOs.Account
{
    public record UpdateProfileRequest
    {
        [Description("Required, Minimum Length is 3 characters!")]
        [Required]
        [MinLength(3)]
        public string UserName { get; set; } = null!;
        [Description("Required, Must be a valid email format!")]
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Description("Required, Minimum Length is 3 characters!")]
        [Required]
        [MinLength(3)]
        public string FullName { get; set; } = null!;
        public IFormFile? ProfilePhoto { get; set; } = null!;
    }
}
