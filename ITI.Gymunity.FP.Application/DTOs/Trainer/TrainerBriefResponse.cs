using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.DTOs.Trainer
{
    public class TrainerBriefResponse
    {
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? ProfilePhotoUrl { get; set; }
        public int TrainerProfileId { get; set; }
        public string Handle { get; set; } = null!;
    }
}
