using ITI.Gymunity.FP.Domain.Models.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Domain.RepositoiesContracts.ClientRepositories
{
    public interface IWorkoutLogRepository : IRepository<WorkoutLog>
    {
        Task<WorkoutLog?> GetByIdAsync(long id);
    }
}
