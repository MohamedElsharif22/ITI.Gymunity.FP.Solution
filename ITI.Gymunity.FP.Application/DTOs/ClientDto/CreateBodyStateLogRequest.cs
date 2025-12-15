using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.DTOs.ClientDto
{
    public class CreateBodyStateLogRequest
    {
        public decimal? WeightKg { get; set; }
        public decimal? BodyFatPercent { get; set; }
        public string? MeasurementsJson { get; set; } // { "neck": 40, "waist": 80, ... }
        public string? PhotoFrontUrl { get; set; }
        public string? PhotoSideUrl { get; set; }
        public string? PhotoBackUrl { get; set; }
        public string? Notes { get; set; }

    }
}
