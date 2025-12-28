using ITI.Gymunity.FP.Domain.Models.Trainer;

namespace ITI.Gymunity.FP.Domain.RepositoiesContracts
{
    public interface IPackageRepository : IRepository<Package>
    {
        Task<IReadOnlyList<Package>> GetByTrainerIdAsync(int trainerId);
        Task<IReadOnlyList<Package>> GetAllActiveWithProgramsAsync();
        Task<Package?> GetByIdWithProgramsAsync(int id);
    }
}
