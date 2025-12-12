
using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.Program;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;

namespace ITI.Gymunity.FP.Application.Services
{
    public class ExerciseLibraryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ExerciseLibraryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ExerciseResponse> CreateExercise(CreateExerciseRequest request)
        {
            var exercise = _mapper.Map<Exercise>(request);
            exercise.IsCustom = true;
            exercise.IsApproved = false;

            var repository = _unitOfWork.Repository<Exercise>();
            repository.Add(exercise);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ExerciseResponse>(exercise);
        }

        public async Task<IEnumerable<ExerciseResponse>> GetAllExercises(string? trainerId = null, bool? isApproved = null)
        {
            var repository = _unitOfWork.Repository<Exercise>();
            var exercises = await repository.GetAllAsync();

            var filteredExercises = exercises
                .Where(e => !e.IsDeleted)
                .Where(e => trainerId == null || e.TrainerId == trainerId)
                .Where(e => isApproved == null || e.IsApproved == isApproved);

            return filteredExercises.Select(e => _mapper.Map<ExerciseResponse>(e));
        }

        public async Task<ExerciseResponse?> GetExerciseById(int exerciseId)
        {
            var repository = _unitOfWork.Repository<Exercise>();
            var exercise = await repository.GetByIdAsync(exerciseId);

            if (exercise == null || exercise.IsDeleted)
            {
                return null;
            }

            return _mapper.Map<ExerciseResponse>(exercise);
        }

        public async Task<IEnumerable<ExerciseResponse>> GetGlobalExercises()
        {
            var repository = _unitOfWork.Repository<Exercise>();
            var exercises = await repository.GetAllAsync();

            var globalExercises = exercises
                .Where(e => !e.IsDeleted && !e.IsCustom);

            return globalExercises.Select(e => _mapper.Map<ExerciseResponse>(e));
        }

        public async Task<IEnumerable<ExerciseResponse>> GetTrainerExercises(string trainerId)
        {
            var repository = _unitOfWork.Repository<Exercise>();
            var exercises = await repository.GetAllAsync();

            var trainerExercises = exercises
                .Where(e => !e.IsDeleted && e.IsCustom && e.TrainerId == trainerId);

            return trainerExercises.Select(e => _mapper.Map<ExerciseResponse>(e));
        }

        public async Task<IEnumerable<ExerciseResponse>> GetPendingApprovals()
        {
            var repository = _unitOfWork.Repository<Exercise>();
            var exercises = await repository.GetAllAsync();

            var pendingExercises = exercises
                .Where(e => !e.IsDeleted && e.IsCustom && !e.IsApproved);

            return pendingExercises.Select(e => _mapper.Map<ExerciseResponse>(e));
        }
    }
}