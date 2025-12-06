using ITI.Gymunity.FP.Domain.Models.Trainer;
<<<<<<< HEAD
=======
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
>>>>>>> main

namespace ITI.Gymunity.FP.Domain.RepositoiesContracts
{
    public interface ITrainerProfileRepository : IRepository<TrainerProfile>
    {
        Task<TrainerProfile?> GetByHandleAsync(string handle);
        Task<bool> HandleExistsAsync(string handle);
        Task<IReadOnlyList<TrainerProfile>> GetTopRatedTrainersAsync(int count);
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> main
