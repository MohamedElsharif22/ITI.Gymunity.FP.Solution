using ITI.Gymunity.FP.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.DTOs.ClientDto
{
    public class ClientProfileResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        //public string? PhoneNumber { get; set; }
        //public string FullName { get; set; }
        public int? HeightCm { get; set; }
        public decimal? StartingWeightKg { get; set; }
        public Gender? Gender { get; set; }
        public ClientGoal? Goal { get; set; }  // "Fat Loss", "Muscle Gain", etc.
        public ExperienceLevel? ExperienceLevel { get; set; }  // Beginner, Intermediate, Advanced

        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public BodyStateLogResponse? BodyStateLog { get; set; }
    }
}
