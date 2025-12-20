
using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain.Specification;

namespace ITI.Gymunity.FP.Infrastructure.Specefications.Trainer
{
    public class TrainerByUserIdSpecs : BaseSpecification<TrainerProfile>
    {
        public TrainerByUserIdSpecs(string userId)
            : base(t => t.UserId == userId)  // ✅ UserId مش Id
        {
            AddInclude(t => t.User);  // لو محتاج بيانات الـ User
        }
    }
}