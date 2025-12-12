using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.DTOs.Program
{

    // Program Detail Response (with weeks)
    public class ProgramDetailResponse : ProgramResponse
    {
        public List<ProgramWeekResponse> Weeks { get; set; } = new();
    }
}
