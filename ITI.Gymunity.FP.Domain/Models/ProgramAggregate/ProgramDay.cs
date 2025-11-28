using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Domain.Models.ProgramAggregate
{
    public class ProgramDay : BaseEntity
    {
        public int ProgramWeekId { get; set; }
        public int DayNumber { get; set; } // 1–7
        public string? Title { get; set; } // "Lower Body A", "Rest", etc.
        public string? Notes { get; set; }

        public ProgramWeek Week { get; set; } = null!;
        public ICollection<ProgramDayExercise> Exercises { get; set; } = [];
    }
}
