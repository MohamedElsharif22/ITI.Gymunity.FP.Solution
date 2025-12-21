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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
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

            ValidateMeasurementsJson(request.MeasurementsJson);

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

        public async Task<List<BodyStateLogResponse>> GetStateLogsByClientAsync(string userId)
        {
            var specs = new ClientWithUserSpecs(c => c.UserId == userId);

            var profile = await _unitOfWork.Repository<ClientProfile>().GetWithSpecsAsync(specs);

            if (profile == null)
            {
                throw new InvalidOperationException("Client profile not found");
            }

            var bodyStateLogs = profile.BodyStatLogs?.OrderByDescending(b => b.LoggedAt).ToList()
                ?? new List<BodyStatLog> ();

            return _mapper.Map<List<BodyStateLogResponse>>(bodyStateLogs);
        }


        public async Task<BodyStateLogResponse> GetLastStateLog(string userId)
        {
            var specs = new ClientWithUserSpecs(c => c.UserId == userId);

            var profile = await _unitOfWork.Repository<ClientProfile>().GetWithSpecsAsync(specs);
            if(profile == null)
            {
                throw new InvalidOperationException("Client profile not found");
            }

            var lastLog = profile.BodyStatLogs?.OrderByDescending(b => b.LoggedAt).FirstOrDefault();

            if (lastLog == null)
                return null;

            return _mapper.Map<BodyStateLogResponse>(lastLog);
        }

        private void ValidateMeasurementsJson(string? json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;

            try
            {
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.ValueKind != JsonValueKind.Object)
                    throw new ValidationException("Measurements must be a valid JSON object");
            }
            catch (JsonException)
            {
                throw new ValidationException("Measurements must be a valid JSON object");
            }
        }

    }
}
