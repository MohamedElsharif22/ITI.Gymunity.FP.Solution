using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;

namespace ITI.Gymunity.FP.Domain.RepositoiesContracts
{
    public interface IExerciseLibraryRepository : IRepository<Exercise>
    {
        Task<IReadOnlyList<Exercise>> SearchByNameAsync(string? name, string? trainerId = null);
    }
}
