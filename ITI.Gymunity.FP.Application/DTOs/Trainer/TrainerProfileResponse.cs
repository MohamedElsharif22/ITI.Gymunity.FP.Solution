<<<<<<< HEAD
﻿namespace ITI.Gymunity.FP.Application.DTOs.Trainer
=======
﻿using ITI.Gymunity.FP.Domain.Models.Identity;
using ITI.Gymunity.FP.Domain.Models.Messaging;
using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.DTOs.Trainer
>>>>>>> main
{
    public class TrainerProfileResponse
    {
        public int Id { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public string UserName { get; set; } = null!;
<<<<<<< HEAD
        public string Handle { get; set; } = null!;
        public string Bio { get; set; } = string.Empty;
        public string? CoverImageUrl { get; set; }
        public string? VideoIntroUrl { get; set; }
        public string? BrandingColors { get; set; }
=======
        public string Handle { get; set; } = null!; // @wahidfitness  ==> unique
        public string Bio { get; set; } = string.Empty;
        public string? CoverImageUrl { get; set; }
        public string? VideoIntroUrl { get; set; }
        public string? BrandingColors { get; set; } // JSON or hex string
>>>>>>> main
        public bool IsVerified { get; set; } = false;
        public DateTime? VerifiedAt { get; set; }
        public decimal RatingAverage { get; set; } = 0;
        public int TotalClients { get; set; } = 0;
        public int YearsExperience { get; set; }
<<<<<<< HEAD
        public string? StatusImageUrl { get; set; }
        public string? StatusDescription { get; set; }
    }
}
=======

    }
}
>>>>>>> main
