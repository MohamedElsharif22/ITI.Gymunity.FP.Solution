using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs;
using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Application.DTOs.User;
using ITI.Gymunity.FP.Application.Specefications.ClientSpecification.Trainer_Discovery;
using ITI.Gymunity.FP.Application.Specefications.Trainer;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Services.ClientServices
{
    public class ClientTrainersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClientTrainersService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TrainerBriefResponse>> GetClientTrainers(string userId)
        {
            var spec = new TrainersByClientUserIdSpecification(userId);
            var trainers = await _unitOfWork.Repository<TrainerProfile>().GetAllWithSpecsAsync(spec);
            return _mapper.Map<IEnumerable<TrainerBriefResponse>>(trainers);
        }


    }
}




