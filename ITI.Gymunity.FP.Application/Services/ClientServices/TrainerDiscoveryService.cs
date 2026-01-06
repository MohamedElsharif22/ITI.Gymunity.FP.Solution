using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs;
using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Application.DTOs.User;
using ITI.Gymunity.FP.Application.Specefications.ClientSpecification.Trainer_Discovery;
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
    public class TrainerDiscoveryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerDiscoveryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Pagination<TrainerCardDto>> SearchTrainersAsync()
        {
            var trainerSpecs = new TrainerSearchSpecification();
            var trainers = await _unitOfWork.Repository<TrainerProfile>().GetAllWithSpecsAsync(trainerSpecs);

            var totalCount = trainers.Count();
            var paginatedTrainers = trainers
                .Skip((1 - 1) * 10) // PageIndex = 1, PageSize = 10
                .Take(10)
                .ToList();

            var trainerCards = new List<TrainerCardDto>();
            foreach (var trainer in paginatedTrainers)
            {
                var card = _mapper.Map<TrainerCardDto>(trainer);
                trainerCards.Add(card);
            }

            return new Pagination<TrainerCardDto>(1, 10, totalCount, trainerCards);
        }

        /// <summary>
        /// Get detailed profile of a specific trainer by userId
        /// </summary>
        /// <param name="trainerId">The trainer's user ID</param>
        /// <returns>TrainerProfileDetailResponse if found, null otherwise</returns>
        public async Task<TrainerProfileDetailResponse> GetTrainerProfileAsync(string trainerId)
        {
            var spec = new TrainerSearchSpecification();
            var trainers = await _unitOfWork.Repository<TrainerProfile>().GetAllWithSpecsAsync(spec);

            var trainer = trainers.FirstOrDefault(t => t.UserId == trainerId);

            if (trainer == null)
                return null;

            var profile = _mapper.Map<TrainerProfileDetailResponse>(trainer);
            return profile;
        }
    }
}







