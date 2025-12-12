using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.DTOs.Program
{
    // Program Week Response
    public class ProgramWeekResponse
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public int WeekNumber { get; set; }
        public int TotalDays { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
