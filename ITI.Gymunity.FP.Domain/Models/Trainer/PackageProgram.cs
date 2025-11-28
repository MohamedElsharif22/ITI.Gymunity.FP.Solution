using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Domain.Models.Trainer
{
    public class PackageProgram : BaseEntity
    {
        public int PackageId { get; set; }
        public Package Package { get; set; } = null!;

        public int ProgramId { get; set; }
        public Program Program { get; set; } = null!;
    }
}
