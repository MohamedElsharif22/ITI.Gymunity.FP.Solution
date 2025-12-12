using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.Program;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;

namespace ITI.Gymunity.FP.Application.Services
{
    public class ProgramDayService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProgramDayService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProgramDayResponse>> GetAllDays(int? weekId = null)
        {
            var repository = _unitOfWork.Repository<ProgramDay>();
            var days = await repository.GetAllAsync();

            var filteredDays = days
                .Where(d => !d.IsDeleted)
                .Where(d => weekId == null || d.ProgramWeekId == weekId)
                .OrderBy(d => d.DayNumber);

            return filteredDays.Select(d => _mapper.Map<ProgramDayResponse>(d));
        }

        public async Task<ProgramDayResponse?> GetDayById(int dayId)
        {
            var repository = _unitOfWork.Repository<ProgramDay>();
            var day = await repository.GetByIdAsync(dayId);

            if (day == null || day.IsDeleted)
            {
                return null;
            }

            return _mapper.Map<ProgramDayResponse>(day);
        }

        public async Task<ProgramDayDetailResponse?> GetDayDetailById(int dayId)
        {
            var repository = _unitOfWork.Repository<ProgramDay>();
            var days = await repository.GetAllAsync();
            var day = days.FirstOrDefault(d => d.Id == dayId && !d.IsDeleted);

            if (day == null)
            {
                return null;
            }

            return _mapper.Map<ProgramDayDetailResponse>(day);
        }

        public async Task<ProgramDayResponse> UpdateDayInfo(int dayId, string? title, string? notes)
        {
            var repository = _unitOfWork.Repository<ProgramDay>();
            var day = await repository.GetByIdAsync(dayId);

            if (day == null || day.IsDeleted)
            {
                throw new InvalidOperationException("Day not found.");
            }

            if (title != null) day.Title = title;
            if (notes != null) day.Notes = notes;

            day.UpdatedAt = DateTimeOffset.UtcNow;

            repository.Update(day);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ProgramDayResponse>(day);
        }
    }
}
