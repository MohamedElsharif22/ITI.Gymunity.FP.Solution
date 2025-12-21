using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using AutoMapper;
using ITI.Gymunity.FP.Domain;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace ITI.Gymunity.FP.Application.Services
{
 public class TrainerReviewService : ITrainerReviewService
 {
 private readonly ITrainerReviewRepository _repo;
 private readonly IUnitOfWork _uow;
 private readonly IMapper _mapper;

 public TrainerReviewService(ITrainerReviewRepository repo, IUnitOfWork uow, IMapper mapper)
 {
 _repo = repo;
 _uow = uow;
 _mapper = mapper;
 }

 public async Task<TrainerReviewResponse> CreateAsync(string clientUserId, int trainerId, TrainerReviewCreateRequest request)
 {
 // Validate rating
 if (request.Rating <1 || request.Rating >5)
 throw new ArgumentException("Rating must be between1 and5");

 var review = new TrainerReview
 {
 TrainerId = trainerId,
 ClientId = int.Parse(clientUserId), // assumes numeric client id - adapt as needed
 Rating = request.Rating,
 Comment = request.Comment,
 CreatedAt = DateTimeOffset.UtcNow
 };

 _repo.Add(review);
 await _uow.CompleteAsync();

 return _mapper.Map<TrainerReviewResponse>(review);
 }
 }
}
