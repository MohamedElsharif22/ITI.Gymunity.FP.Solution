// ============================================
// ProgramService.cs (UPDATED with file upload)
// Location: Application/Services/
// ============================================
using AutoMapper;
using ITI.Gymunity.FP.Application.Contracts.ExternalServices;
using ITI.Gymunity.FP.Application.DTOs.Program;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;

namespace ITI.Gymunity.FP.Application.Services
{
    public class ProgramService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileUploadService _fileUploadService;
        private const string DEFAULT_PROGRAM_IMAGE = "images/programs/default-program.jpg";

        public ProgramService(IUnitOfWork unitOfWork, IMapper mapper, IFileUploadService fileUploadService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileUploadService = fileUploadService;
        }

        public async Task<ProgramResponse> CreateProgram(CreateProgramRequest request)
        {
            var program = _mapper.Map<Domain.Models.ProgramAggregate.Program>(request);

            // Handle image upload
            if (request.Image != null && _fileUploadService.IsValidImageFile(request.Image))
            {
                program.ImageUrl = await _fileUploadService.UploadImageAsync(request.Image, "programs");
            }
            else
            {
                program.ImageUrl = DEFAULT_PROGRAM_IMAGE;
            }

            // Set default values
            program.IsActive = false;
            program.CreatedAt = DateTime.UtcNow;
            program.UpdatedAt = DateTime.UtcNow;

            // Add program
            var repository = _unitOfWork.Repository<Domain.Models.ProgramAggregate.Program>();
            repository.Add(program);
            await _unitOfWork.CompleteAsync();

            // Create first week with 7 days
            var week = new ProgramWeek
            {
                ProgramId = program.Id,
                WeekNumber = 1
            };

            var weekRepository = _unitOfWork.Repository<ProgramWeek>();
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

            return await GetProgramById(program.Id)
                   ?? throw new InvalidOperationException("Failed to retrieve created program.");
        }

        public async Task<ProgramResponse> UpdateProgram(int programId, UpdateProgramRequest request)
        {
            var repository = _unitOfWork.Repository<Domain.Models.ProgramAggregate.Program>();
            var program = await repository.GetByIdAsync(programId);

            if (program == null || program.IsDeleted)
            {
                throw new InvalidOperationException("Program not found.");
            }

            // Handle image
            if (request.RemoveImage)
            {
                // Delete old image if exists and not default
                if (!string.IsNullOrEmpty(program.ImageUrl) && program.ImageUrl != DEFAULT_PROGRAM_IMAGE)
                {
                    _fileUploadService.DeleteImage(program.ImageUrl);
                }
                program.ImageUrl = DEFAULT_PROGRAM_IMAGE;
            }
            else if (request.Image != null && _fileUploadService.IsValidImageFile(request.Image))
            {
                // Delete old image if exists and not default
                if (!string.IsNullOrEmpty(program.ImageUrl) && program.ImageUrl != DEFAULT_PROGRAM_IMAGE)
                {
                    _fileUploadService.DeleteImage(program.ImageUrl);
                }
                // Upload new image
                program.ImageUrl = await _fileUploadService.UploadImageAsync(request.Image, "programs");
            }

            // Map updated values (excluding image)
            if (!string.IsNullOrEmpty(request.Title))
                program.Title = request.Title;

            if (request.Description != null)
                program.Description = request.Description;

            if (request.Type.HasValue)
                program.Type = request.Type.Value;

            if (request.DurationWeeks.HasValue)
                program.DurationWeeks = request.DurationWeeks.Value;

            if (request.Price.HasValue)
                program.Price = request.Price.Value;

            if (request.IsPublic.HasValue)
                program.IsPublic = request.IsPublic.Value;

            if (request.IsActive.HasValue)
                program.IsActive = request.IsActive.Value;

            if (request.MaxClients.HasValue)
                program.MaxClients = request.MaxClients.Value;

            program.UpdatedAt = DateTime.UtcNow;

            repository.Update(program);
            await _unitOfWork.CompleteAsync();

            return await GetProgramById(program.Id)
                   ?? throw new InvalidOperationException("Failed to retrieve updated program.");
        }

        public async Task<bool> DeleteProgram(int programId)
        {
            var repository = _unitOfWork.Repository<Domain.Models.ProgramAggregate.Program>();
            var program = await repository.GetByIdAsync(programId);

            if (program == null || program.IsDeleted)
            {
                return false;
            }

            // TODO: Check if program has active subscriptions
            // For now, we'll just do soft delete

            // Don't delete the image file - keep it for history
            // If you want to delete: _fileUploadService.DeleteImage(program.ImageUrl);

            program.IsDeleted = true;
            program.UpdatedAt = DateTime.UtcNow;

            repository.Update(program);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<IEnumerable<ProgramResponse>> GetAllPrograms(string? trainerId = null)
        {
            var repository = _unitOfWork.Repository<Domain.Models.ProgramAggregate.Program>();
            var programs = await repository.GetAllAsync();

            var filteredPrograms = programs
                .Where(p => !p.IsDeleted)
                .Where(p => trainerId == null || p.TrainerId == trainerId);

            return filteredPrograms.Select(p => _mapper.Map<ProgramResponse>(p));
        }

        public async Task<ProgramResponse?> GetProgramById(int programId)
        {
            var repository = _unitOfWork.Repository<Domain.Models.ProgramAggregate.Program>();
            var program = await repository.GetByIdAsync(programId);

            if (program == null || program.IsDeleted)
            {
                return null;
            }

            return _mapper.Map<ProgramResponse>(program);
        }

        public async Task<ProgramDetailResponse?> GetProgramDetailById(int programId)
        {
            var repository = _unitOfWork.Repository<Domain.Models.ProgramAggregate.Program>();
            var programs = await repository.GetAllAsync();

            var program = programs.FirstOrDefault(p => p.Id == programId && !p.IsDeleted);

            if (program == null)
            {
                return null;
            }

            return _mapper.Map<ProgramDetailResponse>(program);
        }
    }
}