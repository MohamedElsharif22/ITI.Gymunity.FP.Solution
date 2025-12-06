using AutoMapper;
using ITI.Gymunity.FP.Application.Contracts.ExternalServices;

using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Application.Specefications;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;

namespace ITI.Gymunity.FP.Application.Services
{
    public class TrainerProfileService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IFileUploadService fileUploadService)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IFileUploadService _fileUploadService = fileUploadService;

        public async Task<IEnumerable<TrainerProfileListResponse>> GetAllProfiles()
        {
            var profileSpecs = new TrainerWithUsersAndProgramsSpecs();

            var trainerProfiles = (await _unitOfWork.Repository<TrainerProfile, ITrainerProfileRepository>()
                                                   .GetAllWithSpecsAsync(profileSpecs))
                                                   .Where(tp => !tp.IsDeleted)
                                                   .Select(tp => _mapper.Map<TrainerProfileListResponse>(tp));

            return trainerProfiles;
        }

        public async Task<TrainerProfileDetailResponse?> GetProfileByUserId(string userId)
        {
            var profileSpecs = new TrainerWithUsersAndProgramsSpecs(tp => tp.UserId == userId && !tp.IsDeleted);

            var profile = await _unitOfWork.Repository<TrainerProfile, ITrainerProfileRepository>()
                                          .GetWithSpecsAsync(profileSpecs);

            return profile != null ? _mapper.Map<TrainerProfileDetailResponse>(profile) : null;
        }

        public async Task<TrainerProfileDetailResponse?> GetProfileById(int id)
        {
            var profileSpecs = new TrainerWithUsersAndProgramsSpecs(tp => tp.Id == id && !tp.IsDeleted);

            var profile = await _unitOfWork.Repository<TrainerProfile, ITrainerProfileRepository>()
                                          .GetWithSpecsAsync(profileSpecs);

            return profile != null ? _mapper.Map<TrainerProfileDetailResponse>(profile) : null;
        }

        public async Task<TrainerProfileDetailResponse> CreateProfile(CreateTrainerProfileRequest request)
        {
            // Check if handle already exists
            var repository = _unitOfWork.Repository<TrainerProfile, ITrainerProfileRepository>();
            if (await repository.HandleExistsAsync(request.Handle))
            {
                throw new InvalidOperationException($"Handle '{request.Handle}' already exists.");
            }

            // Map request to entity
            var profile = _mapper.Map<TrainerProfile>(request);

            // Handle cover image upload
            if (request.CoverImage != null)
            {
                profile.CoverImageUrl = await _fileUploadService.UploadImageAsync(
                    request.CoverImage,
                    "trainers/covers");
            }

            // Add profile
            repository.Add(profile);
            await _unitOfWork.CompleteAsync();

            // Return created profile
            return await GetProfileById(profile.Id)
                   ?? throw new InvalidOperationException("Failed to retrieve created profile.");
        }

        public async Task<TrainerProfileDetailResponse> UpdateProfile(int profileId, UpdateTrainerProfileRequest request)
        {
            var profileSpecs = new TrainerWithUsersAndProgramsSpecs(tp => tp.Id == profileId && !tp.IsDeleted);
            var repository = _unitOfWork.Repository<TrainerProfile, ITrainerProfileRepository>();

            var profile = await repository.GetWithSpecsAsync(profileSpecs);

            if (profile == null)
            {
                throw new InvalidOperationException("Trainer profile not found.");
            }

            // Check if new handle already exists (if handle is being changed)
            if (!string.IsNullOrEmpty(request.Handle) && request.Handle != profile.Handle)
            {
                if (await repository.HandleExistsAsync(request.Handle))
                {
                    throw new InvalidOperationException($"Handle '{request.Handle}' already exists.");
                }
            }

            // Handle cover image upload
            if (request.CoverImage != null)
            {
                // Delete old image if exists
                if (!string.IsNullOrEmpty(profile.CoverImageUrl))
                {
                    _fileUploadService.DeleteImage(profile.CoverImageUrl);
                }

                profile.CoverImageUrl = await _fileUploadService.UploadImageAsync(
                    request.CoverImage,
                    "trainers/covers");
            }

            // Map updated values
            _mapper.Map(request, profile);
            profile.UpdatedAt = DateTimeOffset.UtcNow;

            repository.Update(profile);
            await _unitOfWork.CompleteAsync();

            return await GetProfileById(profile.Id)
                   ?? throw new InvalidOperationException("Failed to retrieve updated profile.");
        }

        public async Task<bool> DeleteProfile(int profileId)
        {
            var repository = _unitOfWork.Repository<TrainerProfile, ITrainerProfileRepository>();
            var profile = await repository.GetByIdAsync(profileId);

            if (profile == null || profile.IsDeleted)
            {
                return false;
            }

            // Soft delete
            profile.IsDeleted = true;
            profile.UpdatedAt = DateTimeOffset.UtcNow;

            repository.Update(profile);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<TrainerProfileDetailResponse> UpdateStatus(int profileId, UpdateStatusRequest request)
        {
            var profileSpecs = new TrainerWithUsersAndProgramsSpecs(tp => tp.Id == profileId && !tp.IsDeleted);
            var repository = _unitOfWork.Repository<TrainerProfile, ITrainerProfileRepository>();

            var profile = await repository.GetWithSpecsAsync(profileSpecs);

            if (profile == null)
            {
                throw new InvalidOperationException("Trainer profile not found.");
            }

            // Handle status image upload
            if (request.StatusImage != null)
            {
                // Delete old status image if exists
                if (!string.IsNullOrEmpty(profile.StatusImageUrl))
                {
                    _fileUploadService.DeleteImage(profile.StatusImageUrl);
                }

                profile.StatusImageUrl = await _fileUploadService.UploadImageAsync(
                    request.StatusImage,
                    "trainers/status");
            }

            // Update status description
            if (request.StatusDescription != null)
            {
                profile.StatusDescription = request.StatusDescription;
            }

            profile.UpdatedAt = DateTimeOffset.UtcNow;

            repository.Update(profile);
            await _unitOfWork.CompleteAsync();

            return await GetProfileById(profile.Id)
                   ?? throw new InvalidOperationException("Failed to retrieve updated profile.");
        }

        public async Task<bool> DeleteStatus(int profileId)
        {
            var repository = _unitOfWork.Repository<TrainerProfile, ITrainerProfileRepository>();
            var profile = await repository.GetByIdAsync(profileId);

            if (profile == null || profile.IsDeleted)
            {
                return false;
            }

            // Delete status image if exists
            if (!string.IsNullOrEmpty(profile.StatusImageUrl))
            {
                _fileUploadService.DeleteImage(profile.StatusImageUrl);
            }

            // Clear status fields
            profile.StatusImageUrl = null;
            profile.StatusDescription = null;
            profile.UpdatedAt = DateTimeOffset.UtcNow;

            repository.Update(profile);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
