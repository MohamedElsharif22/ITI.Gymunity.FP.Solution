using ITI.Gymunity.FP.Domain.Models.Client;
using ITI.Gymunity.FP.Domain.RepositoiesContracts.ClientRepositories;
using ITI.Gymunity.FP.Infrastructure._Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Infrastructure.Repositories.ClientRepositories
{
    internal class WorkoutLogRepository(AppDbContext dbcontext)
        : Repository<WorkoutLog>(dbcontext), IWorkoutLogRepository
    {
        private readonly AppDbContext _dbcontext = dbcontext;

        async Task<WorkoutLog?> IWorkoutLogRepository.GetByIdAsync(long id) 
            => await _dbcontext.FindAsync<WorkoutLog>(id);
    }
}
