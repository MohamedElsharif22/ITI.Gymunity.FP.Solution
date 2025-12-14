using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.Client;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models.Identity;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Services
{
 public interface IClientService
 {
 Task<IReadOnlyList<ClientGetAllResponse>> GetAllByTrainerIdAsync(string trainerId);
 Task<ClientGetByIdResponse?> GetByIdAsync(string userId);
 }

 public class ClientService : IClientService
 {
 private readonly IUnitOfWork _unitOfWork;
 private readonly IMapper _mapper;
 private readonly UserManager<AppUser> _userManager;

 public ClientService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
 {
 _unitOfWork = unitOfWork;
 _mapper = mapper;
 _userManager = userManager;
 }

 public async Task<IReadOnlyList<ClientGetAllResponse>> GetAllByTrainerIdAsync(string trainerId)
 {
 // Replace with real logic linking clients to trainer (subscriptions, packages, etc.)
 var users = await _userManager.Users.ToListAsync();
 return users.Select(u => new ClientGetAllResponse { UserId = u.Id, UserName = u.UserName ?? string.Empty, ProfilePhotoUrl = u.ProfilePhotoUrl, Role = u.Role.ToString() }).ToList();
 }

 public async Task<ClientGetByIdResponse?> GetByIdAsync(string userId)
 {
 var user = await _userManager.FindByIdAsync(userId);
 if (user == null) return null;
 return new ClientGetByIdResponse { UserId = user.Id, UserName = user.UserName ?? string.Empty, ProfilePhotoUrl = user.ProfilePhotoUrl, Role = user.Role.ToString(), LastLoginAt = user.LastLoginAt, StripeCustomerId = user.StripeCustomerId };
 }
 }
}
