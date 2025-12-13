using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.ClientDto;
using ITI.Gymunity.FP.Application.Specefications.ClientSpecification;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models.Client;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Models.Identity;
using ITI.Gymunity.FP.Domain.RepositoiesContracts.ClientRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ITI.Gymunity.FP.Application.Services.ClientServices
{
    public class ClientProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ClientProfileService> _logger;
        private readonly UserManager<AppUser> _userManager;

        public ClientProfileService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ClientProfileService> logger, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
        }


        public async Task<ClientProfileResponse?> GetClientProfileAsync(string userId)
        {
            var clientSpec = new ClientWithUserSpecs(c => c.UserId == userId);

            var clientProfile = await _unitOfWork.Repository<ClientProfile, IClientProfileRepository>().
                GetWithSpecsAsync(clientSpec);

            return _mapper.Map<ClientProfileResponse>(clientProfile);
        }


        public async Task<ClientProfileResponse?> CreateClientProfileAsync(string userId, ClientProfileRequest request)
        {
            var spec = new ClientWithUserSpecs(c => c.UserId == userId);

            var existingProfile = await _unitOfWork.Repository<ClientProfile, IClientProfileRepository>()
                .GetWithSpecsAsync(spec);

            if (existingProfile != null)
                return _mapper.Map<ClientProfileResponse>(existingProfile);


            var currentUser = await _userManager.FindByIdAsync(userId);
            if (currentUser == null)
                return null;

            var clientProfile = _mapper.Map<ClientProfile>(request);
            clientProfile.UserId = userId;
            clientProfile.CreatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<ClientProfile, IClientProfileRepository>().Add(clientProfile);

            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ClientProfileResponse>(clientProfile);
        }
        public async Task<ClientProfileResponse?> UpdateClientProfileAsync(string userId, ClientProfileRequest request)
        {
            var spec = new ClientWithUserSpecs(c => c.UserId == userId);

            var clientProfile = await _unitOfWork.Repository<ClientProfile, IClientProfileRepository>()
                .GetWithSpecsAsync(spec);

            if (clientProfile is null)
                return null;

            _mapper.Map(request,clientProfile);
          
            _unitOfWork.Repository<ClientProfile>().Update(clientProfile);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch
            {
                return null;
            }

            return _mapper.Map<ClientProfileResponse>(clientProfile);
        }
        //public async Task<bool> UpdateClientInfoAsync(string userId, string photoUrl)
        //{

        //    _userManager.FindByIdAsync(userId);

        //    await _unitOfWork.CompleteAsync();
        //    return true;
        //}

        //public async Task<bool> UpdateClientGoalAsync(string userId, string goal)
        //{
        //    var spec = new ClientWithUserSpecs(c => c.UserId == userId);

        //    var clientProfile = await _unitOfWork.Repository<ClientProfile, IClientProfileRepository>().
        //        GetWithSpecsAsync(spec);

        //    if (clientProfile is null)
        //        return false;

            

        //    await _unitOfWork.CompleteAsync();
        //    return true;
        //}
        //public async Task<bool> UpdateExperienceLevelAsync(string userId, string level)
        //{
        //    var spec = new ClientWithUserSpecs(c => c.UserId == userId);

        //    var clientProfile = await _unitOfWork.Repository<ClientProfile, IClientProfileRepository>().
        //        GetWithSpecsAsync(spec);

        //    if (clientProfile is null)
        //        return false;

        //    clientProfile.ExperienceLevel = level;

        //    await _unitOfWork.CompleteAsync();
        //    return true;
        //}
        //public async Task<bool> UpdateAnthropometricsAsync(string userId, int? height, decimal? weight)
        //{
        //    var spec = new ClientWithUserSpecs(c => c.UserId == userId);

        //    var clientProfile = await _unitOfWork.Repository<ClientProfile, IClientProfileRepository>().
        //        GetWithSpecsAsync(spec);

        //    if (clientProfile is null)
        //        return false;

        //    clientProfile.HeightCm = height;
        //    clientProfile.StartingWeightKg = weight;

        //    await _unitOfWork.CompleteAsync();
        //    return true;
        //}
        public async Task<bool> IsProfileCompletedAsync(string userId)
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

            return isCompleted;
        }
        public async Task<bool> DeleteProfileAsync(string userId)
        {
            var spec = new ClientWithUserSpecs(c => c.UserId == userId);

            var existingProfile = await _unitOfWork.Repository<ClientProfile, IClientProfileRepository>()
                .GetWithSpecsAsync(spec);

            if(existingProfile == null) 
                return false;

            _unitOfWork.Repository<ClientProfile>().Delete(existingProfile);
            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch
            {
                _logger.LogError("Error occured while Saving Changes");
                return false;
            }
            return true;

        }
    }
}
