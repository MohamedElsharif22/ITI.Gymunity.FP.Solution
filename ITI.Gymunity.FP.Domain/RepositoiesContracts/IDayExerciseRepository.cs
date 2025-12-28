using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;

namespace ITI.Gymunity.FP.Domain.RepositoiesContracts
{
    public interface IDayExerciseRepository : IRepository<ProgramDayExercise>
    {
        Task<IReadOnlyList<ProgramDayExercise>> GetByDayIdAsync(int dayId);
    }
}
