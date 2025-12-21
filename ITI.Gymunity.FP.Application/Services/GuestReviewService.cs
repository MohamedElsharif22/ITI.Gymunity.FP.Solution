using ITI.Gymunity.FP.Application.DTOs.Guest;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using AutoMapper;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace ITI.Gymunity.FP.Application.Services
{
 public class GuestReviewService : IGuestReviewService
 {
 private readonly IGuestReviewRepository _repo;
 private readonly IUnitOfWork _uow;
 private readonly IMapper _mapper;

 public GuestReviewService(IGuestReviewRepository repo, IUnitOfWork uow, IMapper mapper)
 {
 _repo = repo;
 _uow = uow;
 _mapper = mapper;
 }

 public async Task<GuestReviewsByTrainerResponse> GetApprovedReviewsByTrainerAsync(int trainerProfileId)
 {
 // Use unit of work to obtain repo (per requirement)
 var repo = _uow.Repository<TrainerReview, IGuestReviewRepository>();
 var reviews = await repo.GetApprovedByTrainerIdAsync(trainerProfileId);
 var items = reviews.Select(r => _mapper.Map<GuestReviewResponseItem>(r)).ToArray();
 return new GuestReviewsByTrainerResponse { TrainerProfileId = trainerProfileId, Reviews = items };
 }

 public async Task<TopTrainersResponse> GetTopTrainersAsync()
 {
 var topList = await _repo.GetTopTrainersByClientsAsync(10);
 var dtos = topList.Select(tp => _mapper.Map<TopTrainerResponse>(tp)).ToArray();
 return new TopTrainersResponse { Trainers = dtos };
 }
 }
}
