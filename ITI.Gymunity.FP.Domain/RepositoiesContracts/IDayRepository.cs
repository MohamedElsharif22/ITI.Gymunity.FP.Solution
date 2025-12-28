using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;

namespace ITI.Gymunity.FP.Domain.RepositoiesContracts
{
    public interface IDayRepository : IRepository<ProgramDay>
    {
        Task<IReadOnlyList<ProgramDay>> GetByWeekIdAsync(int weekId);
        Task<ProgramDay?> GetWithExercisesAsync(int id);
    }
}
