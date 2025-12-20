using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.DTOs.ClientDto
{
    public class OnboardingRequest
    {
        public int? HeightCm { get; set; }
        public decimal? StartingWeightKg { get; set; }
        public string? Goal { get; set; }
        public string? ExperienceLevel { get; set; }
    }
}
