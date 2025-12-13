using ITI.Gymunity.FP.Domain.Models.Client;
using ITI.Gymunity.FP.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Domain.RepositoiesContracts.ClientRepositories
{
    public interface IClientProfileRepository: IRepository<ClientProfile>
    {
        //Task<ClientProfile?> GetByUserIdAsync(string userId);
        //public void UpdateClientPhotoAsync(AppUser user, string photoUrl);
        //Task<bool> IsClientProfileCompletedAsync(string userId);
    }
}
