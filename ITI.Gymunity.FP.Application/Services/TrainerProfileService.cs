using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Application.Specefications;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Services
{
    public class TrainerProfileService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<TrainerProfileResponse>> GetAllProfiles()
        {
            var profileSpecs = new TrainerWithUsersAndProgramsSpecs();

            var trainerProfiles = (await _unitOfWork.Repository<TrainerProfile, ITrainerProfileRepository>()
                                                   .GetAllWithSpecsAsync(profileSpecs))
                                                   .Select(tp => _mapper.Map<TrainerProfileResponse>(tp));

            return trainerProfiles;

        }


        public async Task<TrainerProfileResponse?> GetProfileById(int id)
        {
            var spec = new TrainerProfileByIdSpecs(id);

            var trainer = await _unitOfWork
                                .Repository<TrainerProfile, ITrainerProfileRepository>()
                                .GetWithSpecsAsync(spec);

            if (trainer is null)
                return null;

            return _mapper.Map<TrainerProfileResponse>(trainer);
        }

        public async Task<TrainerProfileResponse?> CreateProfileAsync(CreateTrainerProfileRequest request)
        {
            var repo = _unitOfWork.Repository<TrainerProfile, ITrainerProfileRepository>();

            // 1) Check if handle exists
            var handleExists = await repo.HandleExistsAsync(request.Handle);
            if (handleExists)
                throw new Exception("Handle already exists.");

            // 2) Create entity
            var entity = _mapper.Map<TrainerProfile>(request);

            // 3) Default values
            entity.IsVerified = false;
            entity.RatingAverage = 0;
            entity.TotalClients = 0;

            // 4) Save
            repo.Add(entity);
            await _unitOfWork.CompleteAsync();

            // 5) Return mapped response
            return _mapper.Map<TrainerProfileResponse>(entity);
        }


    }
}
