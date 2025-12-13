using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.DTOs.ClientDto
{
    public abstract class ClientProfileRequest
    {
        [Required]
        public string userId { get; set; }
        public string UserName { get; set; } = null!;
        //public string? PhoneNumber { get; set; }
        //public string FullName { get; set; }
        //public string? PhotoURL { get; set; }
        public int? HeightCm { get; set; }
        public decimal? StartingWeightKg { get; set; }
        public string? Gender { get; set; }
        public string? Goal { get; set; } // "Fat Loss", "Muscle Gain", etc.
        public string? ExperienceLevel { get; set; } // Beginner, Intermediate, Advanced
        public abstract IFormFile Image { get; set; }
    }

    public class CreateClientProfileRequest : ClientProfileRequest
    {
        [Required]
        public override IFormFile Image { get; set; }
    }
    public class UpdateClientProfileRequest : ClientProfileRequest
    {
        
        public override IFormFile? Image { get; set; }
    }


}
