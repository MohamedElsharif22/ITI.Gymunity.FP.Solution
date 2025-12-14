using ITI.Gymunity.FP.Application.DTOs.ClientDto;
using ITI.Gymunity.FP.Application.Specefications.ClientSpecification;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models.Client;
using ITI.Gymunity.FP.Domain.RepositoiesContracts.ClientRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Services.ClientServices
{
    public class OnboardingService(IUnitOfWork unitOfWork)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> IsProfileOnboardingCompletedAsync(string userId)
        {
            var spec = new ClientWithUserSpecs(cp => cp.UserId == userId);

            var clientProfile = await _unitOfWork.Repository<ClientProfile, IClientProfileRepository>()
                                                  .GetWithSpecsAsync(spec);

            if (clientProfile == null)
                return false;

            bool isCompleted =
                clientProfile.HeightCm.HasValue &&
                clientProfile.StartingWeightKg.HasValue &&
                !string.IsNullOrEmpty(clientProfile.Gender) &&
                !string.IsNullOrEmpty(clientProfile.Goal) &&
                !string.IsNullOrEmpty(clientProfile.ExperienceLevel);

            clientProfile.IsOnboardingCompleted = true;
            return isCompleted;
        }

        public async Task<bool> CompleteOnboardingAsync(string userId, OnboardingRequest request)
        {
            var clientSpec = new ClientWithUserSpecs(c => c.UserId == userId);

            var profile = await _unitOfWork.Repository<ClientProfile, IClientProfileRepository>()
                .GetWithSpecsAsync(clientSpec);

            if (profile == null)
                throw new Exception("Client profile not found");

            if (profile.IsOnboardingCompleted)
                return false;

            profile.HeightCm = request.HeightCm;
            profile.StartingWeightKg = request.StartingWeightKg;
            profile.Goal = request.Goal;
            profile.ExperienceLevel = request.ExperienceLevel;
            profile.IsOnboardingCompleted = true;

            _unitOfWork.Repository<ClientProfile>().Update(profile);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch
            {
                
            }
            return true;
        }
    }
} 
