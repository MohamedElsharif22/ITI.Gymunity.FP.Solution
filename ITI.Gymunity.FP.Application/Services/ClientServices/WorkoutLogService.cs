using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.ClientDto;
using ITI.Gymunity.FP.Application.Specefications.ClientSpecification;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Client;
using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using ITI.Gymunity.FP.Domain.RepositoiesContracts.ClientRepositories;
using ITI.Gymunity.FP.Domain.Models.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Services.ClientServices
{
    public class WorkoutLogService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<WorkoutLog> logger)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<WorkoutLog> _logger = logger;

        public async Task<WorkoutLogResponse> AddWorkoutLogAsync(string userId, WorkoutLogRequest request) 
        {
            var specs = new ClientWithUserSpecs(c => c.UserId == userId);

            var profile = await _unitOfWork.Repository<ClientProfile, IClientProfileRepository>()
                .GetWithSpecsAsync(specs);

            if (profile == null)
            {
                _logger.LogWarning("Client profile not found for UserId: {UserId}", userId);
                throw new InvalidOperationException("Client profile not found");
            }

            var programDay = await _unitOfWork.Repository<ProgramDay>().GetByIdAsync(request.ProgramDayId);

            if (programDay == null)
            {
                _logger.LogWarning("ProgramDay {ProgramDayId} not found", request.ProgramDayId);
                throw new InvalidOperationException($"Program day with ID {request.ProgramDayId} not found");
            }


            // Verify client has access to this program via active subscription

            var subscriptionSpecs = new SubscriptionWithClientAndProgramSpecs(s => s.ClientId == userId &&
            s.Status == SubscriptionStatus.Active);

            var hasAccess = await _unitOfWork.Repository<Subscription>()
                .GetAllWithSpecsAsync(subscriptionSpecs);

            if (!hasAccess.Any())
            {
                _logger.LogWarning("Client {UserId} does not have an active subscription", userId);
                throw new InvalidOperationException("Client does not have an active subscription");
            }

            var workoutLog = _mapper.Map<WorkoutLog>(request);
            workoutLog.ClientProfileId = profile.Id;

            //ValidateExercisesJson(workoutLog.ExercisesLoggedJson);

            _unitOfWork.Repository<WorkoutLog>().Add(workoutLog);

            try
            {
                await _unitOfWork.CompleteAsync();
                _logger.LogInformation("WorkoutLog created successfully for ClientId: {ClientId}, ProgramDayId: {ProgramDayId}",
                    profile.Id, request.ProgramDayId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving WorkoutLog for ClientId: {ClientId}, ProgramDayId: {ProgramDayId}",
                    profile.Id, request.ProgramDayId);
                throw;
            }

            return _mapper.Map<WorkoutLogResponse>(workoutLog);
        }


        public async Task<WorkoutLogResponse?> GetWorkoutLogByIdAsync(string userId, long workoutLogId)
        {
            var profileSpecs = new ClientWithUserSpecs(c => c.UserId == userId);

            var profile = await _unitOfWork.Repository<ClientProfile>().GetWithSpecsAsync(profileSpecs);
            if (profile == null)
            {
                _logger.LogWarning("Client profile not found for UserId: {UserId}", userId);
                throw new InvalidOperationException("Client Profile not found.");
            }

            var workoutLogSpecs = new WorkoutLogWithDetailsSpecs(w => w.Id == workoutLogId &&
            w.ClientProfileId == profile.Id);

            var workoutLog = await _unitOfWork.Repository<WorkoutLog>().GetWithSpecsAsync(workoutLogSpecs);

            return workoutLog == null ? null : _mapper.Map<WorkoutLogResponse>(workoutLog);
        }
    }
}
