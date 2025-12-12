using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.DTOs.Program
{
    // Program Day Detail Response (with exercises)
    public class ProgramDayDetailResponse : ProgramDayResponse
    {
        public List<ProgramDayExerciseResponse> Exercises { get; set; } = new();
    }
}
