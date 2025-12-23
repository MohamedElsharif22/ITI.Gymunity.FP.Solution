using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.DTOs.ClientDto
{
    public class ClientProfileRequest
    {
        //public int Id { get; set; }
        [Required(ErrorMessage = "User name is required")]
        [StringLength(50, MinimumLength = 3,
        ErrorMessage = "User name must be between 3 and 50 characters")]
        public string UserName { get; set; } = null!;

        [Range(50, 300, ErrorMessage = "Height must be between 50 and 300 cm")]
        public int? HeightCm { get; set; }

        [Range(20, 500, ErrorMessage = "Weight must be between 20 and 500 kg")]
        public decimal? StartingWeightKg { get; set; }

        [RegularExpression("^(Male|Female)$",
            ErrorMessage = "Gender must be either Male or Female")]
        public string? Gender { get; set; }

        [RegularExpression("^(Fat Loss|Muscle Gain|Maintenance)$",
            ErrorMessage = "Goal must be Fat Loss, Muscle Gain, or Maintenance")]
        public string? Goal { get; set; }  // "Fat Loss", "Muscle Gain", etc.

        [RegularExpression("^(Beginner|Intermediate|Advanced)$",
            ErrorMessage = "Experience level must be Beginner, Intermediate, or Advanced")]
        public string? ExperienceLevel { get; set; }  // Beginner, Intermediate, Advanced

        //public string userId { get; set; }
        //public string? PhoneNumber { get; set; }
        //public string FullName { get; set; }
        //public string? PhotoURL { get; set; }
        //public abstract IFormFile Image { get; set; }
    }

    //public class CreateClientProfileRequest : ClientProfileRequest
    //{
    //    [Required]
    //    public override IFormFile Image { get; set; }
    //}
    //public class UpdateClientProfileRequest : ClientProfileRequest
    //{
        
    //    public override IFormFile? Image { get; set; }
    //}


}
