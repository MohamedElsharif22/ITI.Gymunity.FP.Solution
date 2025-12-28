using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.DTOs.Program.ProgramDayDtos
{
    public class ProgramDayResponse
    {
        public int Id { get; set; }
        public int ProgramWeekId { get; set; }
        public int DayNumber { get; set; } // 1–7
        public string? Title { get; set; } // "Lower Body A", "Rest", etc.
        public string? Notes { get; set; }

        public List<ProgramDayExerciseResponse> Exercises { get; set; }

    }
}
