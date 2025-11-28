using ITI.Gymunity.FP.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Domain.Models.Client
{
    public class BodyStatLog : BaseEntity 
    {
        public string ClientId { get; set; } = null!;
        public DateTime LoggedAt { get; set; } = DateTime.UtcNow;
        public decimal? WeightKg { get; set; }
        public decimal? BodyFatPercent { get; set; }
        public string? MeasurementsJson { get; set; } // { "neck": 40, "waist": 80, ... }
        public string? PhotoFrontUrl { get; set; }
        public string? PhotoSideUrl { get; set; }
        public string? PhotoBackUrl { get; set; }
        public string? Notes { get; set; }

        public AppUser Client { get; set; } = null!;
    }
}
