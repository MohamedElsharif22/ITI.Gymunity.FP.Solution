using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain.Specification;

namespace ITI.Gymunity.FP.Application.Specefications
{
    public class TrainerProfileByIdSpecs : BaseSpecification<TrainerProfile>
    {
        public TrainerProfileByIdSpecs(int id)
            : base(tp => tp.Id == id)  // Criteria to filter by Id WHERE Id = @id
        {
            // Include User
            AddInclude(tp => tp.User);
            AddInclude(tp => tp.TrainerReviews);
            // Include Programs
            AddInclude(tp => tp.Programs);
        }
    }
}
