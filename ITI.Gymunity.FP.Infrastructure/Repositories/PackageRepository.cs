using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.Gymunity.FP.Infrastructure._Data;

namespace ITI.Gymunity.FP.Infrastructure.Repositories
{
 internal class PackageRepository(AppDbContext dbContext) : Repository<Package>(dbContext), IPackageRepository
 {
 public async Task<IReadOnlyList<Package>> GetByTrainerIdAsync(string trainerId)
 {
 return await _Context.Packages
 .Where(p => p.TrainerId == trainerId && !p.IsDeleted)
 .Include(p => p.PackagePrograms)
 .ThenInclude(pp => pp.Program)
 .ToListAsync();
 }

 public async Task<IReadOnlyList<Package>> GetAllActiveWithProgramsAsync()
 {
 return await _Context.Packages
 .Where(p => p.IsActive && !p.IsDeleted)
 .Include(p => p.PackagePrograms)
 .ThenInclude(pp => pp.Program)
 .Include(p => p.Trainer)
 .ToListAsync();
 }

 public async Task<Package?> GetByIdWithProgramsAsync(int id)
 {
 return await _Context.Packages
 .Where(p => p.Id == id && !p.IsDeleted)
 .Include(p => p.PackagePrograms)
 .ThenInclude(pp => pp.Program)
 .Include(p => p.Trainer)
 .FirstOrDefaultAsync();
 }
 }
}
