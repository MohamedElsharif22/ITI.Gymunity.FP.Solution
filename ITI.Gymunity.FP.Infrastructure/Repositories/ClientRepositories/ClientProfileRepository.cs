using ITI.Gymunity.FP.Domain.Models.Client;
using ITI.Gymunity.FP.Domain.Models.Identity;
using ITI.Gymunity.FP.Domain.RepositoiesContracts.ClientRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITI.Gymunity.FP.Infrastructure._Data;

namespace ITI.Gymunity.FP.Infrastructure.Repositories.ClientRepositories
{
    internal class ClientProfileRepository(AppDbContext dbContext) :
        Repository<ClientProfile>(dbContext), IClientProfileRepository
    {
        //public async Task<bool> IsClientProfileCompletedAsync(string userId)
        //{
        //    var clientProfile = await _Context.ClientProfiles.FirstOrDefaultAsync(cp => cp.UserId == userId);

        //    if (clientProfile == null)
        //        return false;

        //    bool isCompleted = 
        //        clientProfile.HeightCm.HasValue &&
        //        clientProfile.StartingWeightKg.HasValue &&
        //        clientProfile.Gender != null &&
        //        clientProfile.Goal != null &&
        //        clientProfile.ExperienceLevel != null;

        //    return isCompleted;
        //}
        //public void UpdateClientPhotoAsync(AppUser user, string photoUrl)
        //{
        //    user.ProfilePhotoUrl = photoUrl;
        //    _Context.Users.Update(user);
        //}
        //public async Task<ClientProfile?> GetByUserIdAsync(string userId)      
        //    => await _Context.ClientProfiles.FirstOrDefaultAsync(cp => cp.UserId == userId);
        
    }
}
