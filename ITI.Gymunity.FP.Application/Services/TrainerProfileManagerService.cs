using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain;
using System.Threading.Tasks;
using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Application.Specefications;

namespace ITI.Gymunity.FP.Application.Services
{
 public interface ITrainerProfileManagerService
 {
 Task<TrainerProfileGetResponse?> GetByIdAsync(int id);
 Task<bool> UpdateAsync(int id, UpdateTrainerProfileRequest request);
 }

 public class TrainerProfileManagerService : ITrainerProfileManagerService
 {
 private readonly ITrainerProfileRepository _repo;
 private readonly IUnitOfWork _unitOfWork;
 private readonly IMapper _mapper;

 public TrainerProfileManagerService(ITrainerProfileRepository repo, IUnitOfWork unitOfWork, IMapper mapper)
 {
 _repo = repo;
 _unitOfWork = unitOfWork;
 _mapper = mapper;
 }

 public async Task<TrainerProfileGetResponse?> GetByIdAsync(int id)
 {
 var entity = await _repo.GetWithSpecsAsync(new TrainerWithUsersAndProgramsSpecs(tp => tp.Id == id));
 if (entity == null) return null;
 return _mapper.Map<TrainerProfileGetResponse>(entity);
 }

 public async Task<bool> UpdateAsync(int id, UpdateTrainerProfileRequest request)
 {
 var entity = await _repo.GetByIdAsync(id);
 if (entity == null) return false;

 // Map only provided fields (null checks are handled by DTO if needed)
 if (request.Handle != null) entity.Handle = request.Handle;
 if (request.Bio != null) entity.Bio = request.Bio;
 // CoverImage handled elsewhere (file upload) - keep assignment if provided as URL
 if (request.CoverImage != null)
 {
 // cannot assign IFormFile to string; leave to higher-level service. If DTO contains CoverImageUrl, use that.
 }
 if (request.VideoIntroUrl != null) entity.VideoIntroUrl = request.VideoIntroUrl;
 if (request.BrandingColors != null) entity.BrandingColors = request.BrandingColors;
 if (request.YearsExperience.HasValue) entity.YearsExperience = request.YearsExperience.Value;

 _repo.Update(entity);
 await _unitOfWork.CompleteAsync();
 return true;
 }
 }
}
