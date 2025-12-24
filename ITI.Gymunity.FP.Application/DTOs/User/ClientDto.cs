using ITI.Gymunity.FP.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.DTOs.User
{
    public class ClientDto
    {
        public string Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; }
        public string UserName { get; set; }
        public string? ProfilePhotoUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
    }
}
