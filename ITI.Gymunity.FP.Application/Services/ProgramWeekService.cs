// ============================================
// File 1: ProgramWeekService.cs (COMPLETE)
// Location: Application/Services/
// ============================================
using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.Program;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;

namespace ITI.Gymunity.FP.Application.Services
{
    public class ProgramWeekService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProgramWeekService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProgramWeekResponse> AddWeek(AddWeekRequest request)
        {
            // Check if program exists
            var programRepository = _unitOfWork.Repository<Domain.Models.ProgramAggregate.Program>();
            var program = await programRepository.GetByIdAsync(request.ProgramId);

            if (program == null || program.IsDeleted)
            {
                throw new InvalidOperationException("Program not found.");
            }

            // Get existing weeks count
            var weekRepository = _unitOfWork.Repository<ProgramWeek>();
            var allWeeks = await weekRepository.GetAllAsync();
            var existingWeeks = allWeeks.Where(w => w.ProgramId == request.ProgramId && !w.IsDeleted).ToList();

            int nextWeekNumber = existingWeeks.Any() ? existingWeeks.Max(w => w.WeekNumber) + 1 : 1;

            // Create new week
            var week = new ProgramWeek
            {
                ProgramId = request.ProgramId,
                WeekNumber = nextWeekNumber
            };

            weekRepository.Add(week);
            await _unitOfWork.CompleteAsync();

            // Create 7 days for the week
            var dayRepository = _unitOfWork.Repository<ProgramDay>();
            for (int i = 1; i <= 7; i++)
            {
                var day = new ProgramDay
                {
                    ProgramWeekId = week.Id,
                    DayNumber = i,
                    Title = $"Day {i}"
                };
                dayRepository.Add(day);
            }
            await _unitOfWork.CompleteAsync();

            return await GetWeekById(week.Id)
                   ?? throw new InvalidOperationException("Failed to retrieve created week.");
        }

        public async Task<bool> DeleteWeek(int weekId)
        {
            var weekRepository = _unitOfWork.Repository<ProgramWeek>();
            var week = await weekRepository.GetByIdAsync(weekId);

            if (week == null || week.IsDeleted)
            {
                return false;
            }

            // Soft delete the week
            week.IsDeleted = true;
            week.UpdatedAt = DateTimeOffset.UtcNow;

            // Also soft delete all days
            var dayRepository = _unitOfWork.Repository<ProgramDay>();
            var allDays = await dayRepository.GetAllAsync();
            var days = allDays.Where(d => d.ProgramWeekId == weekId && !d.IsDeleted).ToList();

            foreach (var day in days)
            {
                day.IsDeleted = true;
                day.UpdatedAt = DateTimeOffset.UtcNow;
                dayRepository.Update(day);
            }

            weekRepository.Update(week);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<IEnumerable<ProgramWeekResponse>> GetAllWeeks(int? programId = null)
        {
            var repository = _unitOfWork.Repository<ProgramWeek>();
            var weeks = await repository.GetAllAsync();

            var filteredWeeks = weeks
                .Where(w => !w.IsDeleted)
                .Where(w => programId == null || w.ProgramId == programId)
                .OrderBy(w => w.WeekNumber);

            return filteredWeeks.Select(w => _mapper.Map<ProgramWeekResponse>(w));
        }

        public async Task<ProgramWeekResponse?> GetWeekById(int weekId)
        {
            var repository = _unitOfWork.Repository<ProgramWeek>();
            var week = await repository.GetByIdAsync(weekId);

            if (week == null || week.IsDeleted)
            {
                return null;
            }

            return _mapper.Map<ProgramWeekResponse>(week);
        }

        public async Task<ProgramWeekDetailResponse?> GetWeekDetailById(int weekId)
        {
            var repository = _unitOfWork.Repository<ProgramWeek>();
            var weeks = await repository.GetAllAsync();
            var week = weeks.FirstOrDefault(w => w.Id == weekId && !w.IsDeleted);

            if (week == null)
            {
                return null;
            }

            return _mapper.Map<ProgramWeekDetailResponse>(week);
        }
    }
}
