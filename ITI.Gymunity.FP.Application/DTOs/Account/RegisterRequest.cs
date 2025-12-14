using ITI.Gymunity.FP.Domain.Models.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.DTOs.Account
{
    public record RegisterRequest
    {
        [Required]
        [MinLength(3)]
        public string UserName { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#^()\-_=+{}[\]|;:'"",.<>\/\\]).{8,}$",
            ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; } = null!;
        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = null!;
        [Required]
        [MinLength(3)]
        public string FullName { get; set; } = null!;
        [Required]
        public IFormFile ProfilePhoto { get; set; } = null!;
        [Required]
        [Range(1,2)]
        public byte Role { get; set; }
    }
}
