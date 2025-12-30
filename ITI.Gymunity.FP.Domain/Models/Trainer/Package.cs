using ITI.Gymunity.FP.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Domain.Models.Trainer
{
    public class Package : BaseEntity
    {
        public int TrainerId { get; set; } 

        public TrainerProfile Trainer { get; set; } = null!;

        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public decimal PriceMonthly { get; set; }
        public decimal? PriceYearly { get; set; }
        public string Currency { get; set; } = "EGP";

        public string FeaturesJson { get; set; } = "{}";
        public bool IsActive { get; set; } = true;
        public string? ThumbnailUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // New properties
        //public bool IsAnnual { get; set; } = false;
        public string? PromoCode { get; set; }

        // Many-to-many with Program
        public ICollection<PackageProgram> PackagePrograms { get; set; } = [];
        public ICollection<Subscription> Subscriptions { get; set; } = [];
    }

    /*
    FeaturesJson example:
    {
      "allPrograms": true,
      "formChecksPerWeek": 4,
      "customProgramEveryWeeks": 8,
      "priorityMessaging": true,
      "monthlyVideoCall": true,
      "earlyAccess": true
    }
    */
}
