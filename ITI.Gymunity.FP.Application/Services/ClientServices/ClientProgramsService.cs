using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.Program;
using ITI.Gymunity.FP.Application.DTOs.Program.ProgramDayDtos;
using ITI.Gymunity.FP.Application.Specefications.ClientSpecification;
using ITI.Gymunity.FP.Application.Specefications.ProgramSpecs;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Services.ClientServices
{
    public class ClientProgramsService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ClientProgramsService> logger)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<ClientProgramsService> _logger = logger;
        private readonly IProgramRepository _programRepo = unitOfWork.Repository<Program, IProgramRepository>();


        public async Task<IEnumerable<ProgramResponse>> GetUserActiveProgramsAsync(string userId)
        {
            var subscribtionsSpecs = new ActiveSubscriptionsWithProgramsByUserIdSpecification(userId);
            var subscriptions = await _unitOfWork.Repository<Subscription>().GetAllWithSpecsAsync(subscribtionsSpecs);

            var activePrograms = subscriptions
                        .SelectMany(s => s.Package.PackagePrograms)
                        .Select(async pp => await MapProgramToResponseAsync(pp.Program))
                        .Distinct().ToList();

            if (activePrograms.Count == 0)
            {
                _logger.LogInformation("No active programs found for user with ID {UserId}", userId);
                return [];
            }

            return await Task.WhenAll(activePrograms);
        }

        public async Task<ProgramResponse?> GetProgramByIdAsync(string userId, int programId)
        {
            var hasAccess = await UserHasAccessToProgramAsync(userId, programId);
            if (!hasAccess)
            {
                _logger.LogWarning("User does not have access to program with ID {ProgramId}", programId);
                throw new UnauthorizedAccessException("You do not have access to this program.");
            }
            var program = await _programRepo.GetByIdAsync(programId);
            if (program == null)
            {
                _logger.LogWarning("Program with ID {ProgramId} not found", programId);
                throw new Exception("Program not found.");
            }
            return await MapProgramToResponseAsync(program);
        }

        public async Task<IEnumerable<ProgramWeekResponse>> GetAllWeeks(string userId, int programId)
        {
            var hasAccess = await UserHasAccessToProgramAsync(userId, programId);
            if (!hasAccess)
            {
                _logger.LogWarning("User does not have access to program with ID {ProgramId}", programId);
                throw new UnauthorizedAccessException("You do not have access to this program.");
            }
            var weeksSpecs = new ProgramWeeksByProgramIdSpecification(programId);
            var weeks = await _unitOfWork.Repository<ProgramWeek>().GetAllWithSpecsAsync(weeksSpecs);
            var weekResponses = weeks.Select(w => _mapper.Map<ProgramWeekResponse>(w)).ToList();
            return weekResponses;
        }

        public async Task<IEnumerable<ProgramDayResponse>> GetAllDays(string userId, int weekId)
        {

            var week = await _unitOfWork.Repository<ProgramWeek>().GetByIdAsync(weekId);
            if (week == null)
            {
                _logger.LogWarning("Week with ID {WeekId} not found", weekId);
                throw new Exception("Week not found.");
            }
            var hasAccess = await UserHasAccessToProgramAsync(userId, week.ProgramId);
            if (!hasAccess)
            {
                _logger.LogWarning("User does not have access to program with ID {ProgramId}", week.ProgramId);
                throw new UnauthorizedAccessException("You do not have access to this program.");
            }
            var daysSpecs = new ProgramDaySpec(weekId);
            var days = await _unitOfWork.Repository<ProgramDay>().GetAllWithSpecsAsync(daysSpecs);


            var dayResponses = days.Select(d => _mapper.Map<ProgramDayResponse>(d)).ToList();
            
            return dayResponses;
        }

        public async Task<ProgramDayResponse?> GetDayByIdAsync(string userId, int dayId)
        {
            var daySpecs = new ProgramDaySpec(d => d.Id == dayId);
            var day = await _unitOfWork.Repository<ProgramDay>().GetWithSpecsAsync(daySpecs);
            if (day == null)
            {
                _logger.LogWarning("Day with ID {DayId} not found", dayId);
                throw new Exception("Day not found.");
            }
            var week = await _unitOfWork.Repository<ProgramWeek>().GetByIdAsync(day.ProgramWeekId);
            if (week == null)
            {
                _logger.LogWarning("Week with ID {WeekId} not found for Day ID {DayId}", day.ProgramWeekId, dayId);
                throw new Exception("Week not found.");
            }
            var hasAccess = await UserHasAccessToProgramAsync(userId, week.ProgramId);
            if (!hasAccess)
            {
                _logger.LogWarning("User does not have access to program with ID {ProgramId}", week.ProgramId);
                throw new UnauthorizedAccessException("You do not have access to this program.");
            }
            var dayResponse = _mapper.Map<ProgramDayResponse>(day);
            dayResponse.Exercises = [.. day.Exercises.Select(e => _mapper.Map<ProgramDayExerciseResponse>(e))];
            return dayResponse;
        }






        private async Task<bool> UserHasAccessToProgramAsync(string userId, int programId)
        {
            var subscribtionsSpecs = new ActiveSubscriptionsWithProgramsByUserIdSpecification(userId);
            var subscriptions = await _unitOfWork.Repository<Subscription>().GetAllWithSpecsAsync(subscribtionsSpecs);
            var hasAccess = subscriptions
                        .SelectMany(s => s.Package.PackagePrograms)
                        .Any(pp => pp.ProgramId == programId);
            return hasAccess;
        }

        private async Task<ProgramResponse> MapProgramToResponseAsync(Program program)
        {
            var response = _mapper.Map<ProgramResponse>(program);
            response.DurationWeeks = await _programRepo.GetWeeksCount(program.Id);
            response.TotalExercises = await _programRepo.GetTotalExercisesCount(program.Id);
            return response;
        }

    }
}
