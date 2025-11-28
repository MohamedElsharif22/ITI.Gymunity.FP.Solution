using ITI.Gymunity.FP.Domain.Models.Identity;
using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Domain.Models.Client
{
    public class WorkoutLog : BaseEntity
    {
        public new long Id { get; set; } // long for high volume
        public string ClientId { get; set; } = null!;
        public int ProgramDayId { get; set; }
        public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }
        public int? DurationMinutes { get; set; }
        public string ExercisesLoggedJson { get; set; } = "[]"; // full set/rep/weight log

        public AppUser Client { get; set; } = null!;
        public ProgramDay ProgramDay { get; set; } = null!;
    }
}
