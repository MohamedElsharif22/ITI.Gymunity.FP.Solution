using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.ClientDto;
using ITI.Gymunity.FP.Application.Specefications.ClientSpecification;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models.Client;
using ITI.Gymunity.FP.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Services.ClientServices
{
    public class BodyStateLogService(IUnitOfWork unitOfWork, ILogger<BodyStateLogService> logger, IMapper mapper, UserManager<AppUser> userManager)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<BodyStateLogResponse> AddAsync(string userId, CreateBodyStateLogRequest request)
        {
            var spec = new ClientWithUserSpecs(c => c.UserId == userId);
            var profile = await _unitOfWork.Repository<ClientProfile>().GetWithSpecsAsync(spec);

            if (profile == null)
                throw new InvalidOperationException("Client profile not found");

            var bodyStateLog = _mapper.Map<BodyStatLog>(request);
            bodyStateLog.ClientProfileId = profile.Id;
            bodyStateLog.LoggedAt = DateTime.UtcNow;

            _unitOfWork.Repository<BodyStatLog>().Add(bodyStateLog);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving BodyStateLog");
                throw;
            }

            return _mapper.Map<BodyStateLogResponse>(bodyStateLog);
        } 
    }
}
