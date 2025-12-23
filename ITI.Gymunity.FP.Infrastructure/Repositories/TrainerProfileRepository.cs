using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using Microsoft.EntityFrameworkCore;
using ITI.Gymunity.FP.Infrastructure._Data;

namespace ITI.Gymunity.FP.Infrastructure.Repositories
{
    internal class TrainerProfileRepository(AppDbContext dbContext)
        : Repository<TrainerProfile>(dbContext), ITrainerProfileRepository
    {
        public async Task<TrainerProfile?> GetByHandleAsync(string handle)
        {
            return await _Context.TrainerProfiles
                .FirstOrDefaultAsync(tp => tp.Handle == handle);
        }

        public async Task<IReadOnlyList<TrainerProfile>> GetTopRatedTrainersAsync(int count)
        {
            return await _Context.TrainerProfiles
                .OrderByDescending(tp => tp.RatingAverage)
                .Take(count)
                .ToListAsync();
        }

        public async Task<bool> HandleExistsAsync(string handle)
        {
            return await _Context.TrainerProfiles
                .AnyAsync(tp => tp.Handle == handle);
        }
    }
}
