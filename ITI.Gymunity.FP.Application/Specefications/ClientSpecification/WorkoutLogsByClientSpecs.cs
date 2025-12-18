//using ITI.Gymunity.FP.Domain.Models.Client;
//using ITI.Gymunity.FP.Domain.Specification;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ITI.Gymunity.FP.Application.Specefications.ClientSpecification
//{
//    public class WorkoutLogsByClientSpecs : BaseSpecification<WorkoutLog>
//    {
//        public WorkoutLogsByClientSpecs()
//        {
//            AddInclude(w => w.ProgramDay);
//            AddOrderByDesc(w => w.CompletedAt);
//        }
//        public WorkoutLogsByClientSpecs(int clientProfileId)
//        : base(w => w.ClientProfileId == clientProfileId)
//        {
//            AddInclude(w => w.ProgramDay);
//            AddOrderByDesc(w => w.CompletedAt);
//        }
//    }
//}
