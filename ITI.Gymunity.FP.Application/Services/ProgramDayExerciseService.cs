using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.Program;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;

namespace ITI.Gymunity.FP.Application.Services
{
    public class ProgramDayExerciseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProgramDayExerciseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProgramDayExerciseResponse> AddExercise(AddExerciseToDayRequest request)
        {
            // Check if day exists
            var dayRepository = _unitOfWork.Repository<ProgramDay>();
            var day = await dayRepository.GetByIdAsync(request.ProgramDayId);

            if (day == null || day.IsDeleted)
            {
                throw new InvalidOperationException("Program day not found.");
            }

            // Check if exercise exists
            var exerciseRepository = _unitOfWork.Repository<Exercise>();
            var exercise = await exerciseRepository.GetByIdAsync(request.ExerciseId);

            if (exercise == null || exercise.IsDeleted)
            {
                throw new InvalidOperationException("Exercise not found.");
            }

            // Create day exercise
            var dayExercise = _mapper.Map<ProgramDayExercise>(request);

            var repository = _unitOfWork.Repository<ProgramDayExercise>();
            repository.Add(dayExercise);
            await _unitOfWork.CompleteAsync();

            return await GetExerciseById(dayExercise.Id)
                   ?? throw new InvalidOperationException("Failed to retrieve created exercise.");
        }

        public async Task<ProgramDayExerciseResponse> UpdateExercise(int exerciseId, UpdateExerciseInDayRequest request)
        {
            var repository = _unitOfWork.Repository<ProgramDayExercise>();
            var dayExercise = await repository.GetByIdAsync(exerciseId);

            if (dayExercise == null || dayExercise.IsDeleted)
            {
                throw new InvalidOperationException("Exercise not found.");
            }

            // Map updates (only non-null values)
            if (request.OrderIndex.HasValue)
                dayExercise.OrderIndex = request.OrderIndex.Value;

            if (request.Sets != null)
                dayExercise.Sets = request.Sets;

            if (request.Reps != null)
                dayExercise.Reps = request.Reps;

            if (request.RestSeconds.HasValue)
                dayExercise.RestSeconds = request.RestSeconds.Value;

            if (request.Tempo != null)
                dayExercise.Tempo = request.Tempo;

            if (request.RPE.HasValue)
                dayExercise.RPE = request.RPE.Value;

            if (request.Percent1RM.HasValue)
                dayExercise.Percent1RM = request.Percent1RM.Value;

            if (request.Notes != null)
                dayExercise.Notes = request.Notes;

            if (request.VideoUrl != null)
                dayExercise.VideoUrl = request.VideoUrl;

            if (request.ExerciseDataJson != null)
                dayExercise.ExerciseDataJson = request.ExerciseDataJson;

            dayExercise.UpdatedAt = DateTimeOffset.UtcNow;

            repository.Update(dayExercise);
            await _unitOfWork.CompleteAsync();

            return await GetExerciseById(dayExercise.Id)
                   ?? throw new InvalidOperationException("Failed to retrieve updated exercise.");
        }

        public async Task<bool> DeleteExercise(int exerciseId)
        {
            var repository = _unitOfWork.Repository<ProgramDayExercise>();
            var exercise = await repository.GetByIdAsync(exerciseId);

            if (exercise == null || exercise.IsDeleted)
            {
                return false;
            }

            exercise.IsDeleted = true;
            exercise.UpdatedAt = DateTimeOffset.UtcNow;

            repository.Update(exercise);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<IEnumerable<ProgramDayExerciseResponse>> GetAllExercises(int? dayId = null)
        {
            var repository = _unitOfWork.Repository<ProgramDayExercise>();
            var exercises = await repository.GetAllAsync();

            var filteredExercises = exercises
                .Where(e => !e.IsDeleted)
                .Where(e => dayId == null || e.ProgramDayId == dayId)
                .OrderBy(e => e.OrderIndex);

            return filteredExercises.Select(e => _mapper.Map<ProgramDayExerciseResponse>(e));
        }

        public async Task<ProgramDayExerciseResponse?> GetExerciseById(int exerciseId)
        {
            var repository = _unitOfWork.Repository<ProgramDayExercise>();
            var exercises = await repository.GetAllAsync();
            var exercise = exercises.FirstOrDefault(e => e.Id == exerciseId && !e.IsDeleted);

            if (exercise == null)
            {
                return null;
            }

            return _mapper.Map<ProgramDayExerciseResponse>(exercise);
        }
    }
}
