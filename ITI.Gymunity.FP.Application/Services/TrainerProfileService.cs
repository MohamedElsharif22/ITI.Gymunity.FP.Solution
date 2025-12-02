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

    }
}
