using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.DTOs.Program
{

    // Program Week Detail Response (with days)
    public class ProgramWeekDetailResponse : ProgramWeekResponse
    {
        public List<ProgramDayResponse> Days { get; set; } = new();
    }
}
