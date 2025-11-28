using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Domain.Models.ProgramAggregate
{
    public class ProgramWeek : BaseEntity
    {
        public int ProgramId { get; set; }
        public int WeekNumber { get; set; }

        public Program Program { get; set; } = null!;
        public ICollection<ProgramDay> Days { get; set; } = [];
    }
}
