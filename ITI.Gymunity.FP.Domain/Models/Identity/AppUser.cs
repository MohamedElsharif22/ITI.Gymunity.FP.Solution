using ITI.Gymunity.FP.Domain.Models.Client;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Domain.Models.Identity
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; } = null!;
        public string? ProfilePhotoUrl { get; set; }
        public UserRole Role { get; set; } = UserRole.Client;
        public bool IsVerified { get; set; } = false;
        public string? StripeCustomerId { get; set; }
        public string? StripeConnectAccountId { get; set; } // Only for trainers
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }

        // Navigation
        public TrainerProfile? TrainerProfile { get; set; }
        public ClientProfile? ClientProfile { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public ICollection<WorkoutLog> WorkoutLogs { get; set; } = new List<WorkoutLog>();
        public ICollection<BodyStatLog> BodyStatLogs { get; set; } = new List<BodyStatLog>();
    }
}
