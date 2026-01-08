using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Application.Services;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using ITI.Gymunity.FP.APIs.Responses;
using ITI.Gymunity.FP.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using ITI.Gymunity.FP.Application.Specefications.ClientSpecification;
using ITI.Gymunity.FP.Application.Specefications.Admin;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Enums;

namespace ITI.Gymunity.FP.APIs.Areas.Trainer
{
    [ApiController]
    [Route("api/trainer/[controller]")]
    public class PackagesController : TrainerBaseController
    {
        private readonly IPackageService _service;
        private readonly IUnitOfWork _unitOfWork;

        public PackagesController(IPackageService service, IUnitOfWork unitOfWork)
        {
            _service = service;
            _unitOfWork = unitOfWork;
        }

        // ============================
        // GET: api/trainer/Packages
        // Guest: Get all packages
        // ============================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        // ============================
        // GET: api/trainer/Packages/{id}
        // Guest: Get package by id
        // ============================
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var package = await _service.GetByIdAsync(id);
            if (package == null)
                return NotFound();

            return Ok(package);
        }

        // ============================
        // GET: api/trainer/Packages/byTrainer/{trainerId}
        // Guest: Get packages by trainer profile
        // ============================
        [HttpGet("byTrainer/{trainerId}")]
        public async Task<IActionResult> GetByTrainer(int trainerId)
        {

            var list = await _service.GetAllForTrainerAsync(trainerId);
            return Ok(list);
        }

        // ============================
        // GET: api/trainer/Packages/with-subscriptions
        // Guest: Get trainer's packages with their subscriptions and client info
        // Business logic moved to IPackageService.GetPackagesWithSubscriptionsForTrainerAsync
        // ============================
        [HttpGet("with-subscriptions")]
        public async Task<IActionResult> GetAllWithSubscriptions()
        {
            var currentUserId = GetTrainerId();
            if (string.IsNullOrWhiteSpace(currentUserId))
                return Unauthorized(new { message = "Unauthorized" });

            var trainerRepo = _unitOfWork.Repository<TrainerProfile, ITI.Gymunity.FP.Domain.RepositoiesContracts.ITrainerProfileRepository>();
            var allProfiles = await trainerRepo.GetAllAsync();
            var profile = allProfiles.FirstOrDefault(p => p.UserId == currentUserId && !p.IsDeleted);
            if (profile == null)
                return NotFound(new { message = "Trainer profile not found for current user." });

            var data = await _service.GetPackagesWithSubscriptionsForTrainerAsync(profile.Id);
            return Ok(data);
        }

        // ============================
        // POST: api/trainer/Packages
        // Guest (temporary): Create package
        // trainerId comes from request body
        // ============================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PackageCreateRequestV2 request)
        {
            try
            {
                // Use the V2 creation method so ProgramNames from the request are resolved to program ids
                var created = await _service.CreateAsyncV2(request.TrainerProfileId, request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = created.Id },
                    created
                );
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        // ============================
        // PUT: api/trainer/Packages/{id}
        // Guest (temporary): Update package
        // Returns full PackageResponse on success or when conflict occurs (idempotent)
        // ============================
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PackageCreateRequestV2 request)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, new PackageCreateRequest
                {
                    Name = request.Name,
                    Description = request.Description,
                    PriceMonthly = request.PriceMonthly,
                    PriceYearly = request.PriceYearly,
                    IsActive = request.IsActive,
                    ThumbnailUrl = request.ThumbnailUrl,
                    ProgramIds = request.ProgramIds,
                    IsAnnual = request.IsAnnual,
                    PromoCode = request.PromoCode,
                    TrainerId = request.TrainerProfileId
                });

                if (!updated)
                    return NotFound();

                var pkg = await _service.GetByIdAsync(id);
                if (pkg == null)
                    return NoContent();

                // return full package after update
                return Ok(pkg);
            }
            catch (InvalidOperationException ex)
            {
                // On conflict, return current package as success to make endpoint idempotent
                var pkg = await _service.GetByIdAsync(id);
                if (pkg == null)
                    return NoContent();

                return Ok(pkg);
            }
        }

        // ============================
        // DELETE: api/trainer/Packages/{id}
        // Guest (temporary): Delete package
        // Returns descriptive error messages for invalid requests
        // ============================
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <=0)
            {
                return BadRequest(new { message = "Invalid package id. Id must be a positive integer." });
            }

            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = $"Package with id {id} not found." });

            return NoContent();
        }

        // ============================
        // PATCH: api/trainer/Packages/toggle-active/{id}
        // Guest (temporary): Toggle Active
        // ============================
        [HttpPatch("toggle-active/{id:int}")]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var toggled = await _service.ToggleActiveAsync(id);

            if (!toggled)
                return NotFound();

            return NoContent();
        }

        // ============================
        // GET: api/trainer/Packages/with-profit
        // Trainer: Get packages for current trainer with profit per package and total profit
        // Business logic moved to IPackageService.GetPackagesWithProfitForTrainerAsync
        // ============================
        [HttpGet("with-profit")]
        public async Task<IActionResult> GetMyPackagesWithProfit()
        {
            // Get current trainer user id from base controller
            var currentUserId = GetTrainerId();
            if (string.IsNullOrWhiteSpace(currentUserId))
                return Unauthorized(new { message = "Unauthorized" });

            var trainerRepo = _unitOfWork.Repository<TrainerProfile, ITI.Gymunity.FP.Domain.RepositoiesContracts.ITrainerProfileRepository>();
            var allProfiles = await trainerRepo.GetAllAsync();
            var profile = allProfiles.FirstOrDefault(p => p.UserId == currentUserId && !p.IsDeleted);
            if (profile == null)
                return NotFound(new { message = "Trainer profile not found for current user." });

            var data = await _service.GetPackagesWithProfitForTrainerAsync(profile.Id);
            var total = data.Sum(p => p.Profit);

            return Ok(new { TrainerProfileId = profile.Id, TrainerUserId = currentUserId, Packages = data, TotalProfit = total });
        }
    }
}


//?? ������� ���� ����
//?? ��� �� ������ ��� Guest�

//? ����:

//User

//Claims

//Unauthorized()

//[Authorize]

//?? ��� ������ ��� Parameters �����

//?? ������ ����� (����)

//���� ��� �� Temporary / Development Only
//�� ������� ���� ����:

//Guest:

//GET

//Trainer:

//POST / PUT / DELETE

//������ ��:

//[Authorize(Roles = "Trainer")]
