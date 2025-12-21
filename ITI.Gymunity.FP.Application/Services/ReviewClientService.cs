using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using AutoMapper;
using ITI.Gymunity.FP.Domain;
using System;
using System.Threading.Tasks;
using ITI.Gymunity.FP.Domain.Models.Client;
using System.Linq;

namespace ITI.Gymunity.FP.Application.Services
{
 public class ReviewClientService : IReviewClientService
 {
 private readonly IReviewClientRepository _repo;
 private readonly IUnitOfWork _uow;
 private readonly IMapper _mapper;

 public ReviewClientService(IReviewClientRepository repo, IUnitOfWork uow, IMapper mapper)
 {
 _repo = repo;
 _uow = uow;
 _mapper = mapper;
 }

 public async Task<TrainerReviewResponse> CreateAsync(string clientUserId, int trainerId, TrainerReviewCreateRequest request)
 {
 if (request.Rating <1 || request.Rating >5)
 throw new ArgumentException("Rating must be between1 and5");

 // clientUserId is AppUser.Id (string). Resolve or create ClientProfile for this user.
 var clientRepo = _uow.Repository<ClientProfile>();
 var allClients = await clientRepo.GetAllAsync();
 var clientProfile = allClients.FirstOrDefault(c => c.UserId == clientUserId);
 if (clientProfile == null)
//throw new InvalidOperationException("Client profile not found for the current user.");
//start handle exception by amr
 {
 // Create a minimal ClientProfile so we can store the review
 clientProfile = new ClientProfile
 {
 UserId = clientUserId,
 CreatedAt = DateTimeOffset.UtcNow
 };
 clientRepo.Add(clientProfile);
 await _uow.CompleteAsync();
 }
 //start handle exception by amr
 var review = new TrainerReview
 {
 TrainerId = trainerId,
 ClientId = clientProfile.Id,
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
