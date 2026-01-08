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
            // Get all active subscriptions for the client
            var spec = new ITI.Gymunity.FP.Application.Specefications.Subscriptions.ClientSubscriptionsSpecs(
                userId, 
                ITI.Gymunity.FP.Domain.Models.Enums.SubscriptionStatus.Active);
            
            var subscriptions = await _unitOfWork.Repository<Subscription>().GetAllWithSpecsAsync(spec);

            // Extract unique trainers from subscriptions
            var trainerProfiles = subscriptions
                .Where(s => s.Package?.Trainer != null)
                .Select(s => s.Package!.Trainer)
                .DistinctBy(t => t.Id)
                .ToList();

            // Map to TrainerBriefResponse using AutoMapper
            var result = _mapper.Map<List<TrainerBriefResponse>>(trainerProfiles);

            return result;
        }
    }
}




