using ITI.Gymunity.FP.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Domain.Models.Client
{
    public class ClientProfile : BaseEntity
    {
        public string UserId { get; set; } = null!;
        public int? HeightCm { get; set; }
        public decimal? StartingWeightKg { get; set; }
        public string? Gender { get; set; }
        public string? Goal { get; set; } // "Fat Loss", "Muscle Gain", etc.
        public string? ExperienceLevel { get; set; } // Beginner, Intermediate, Advanced

        public AppUser User { get; set; } = null!;
        public ICollection<Subscription> Subscriptions { get; set; } = [];
    }
}
