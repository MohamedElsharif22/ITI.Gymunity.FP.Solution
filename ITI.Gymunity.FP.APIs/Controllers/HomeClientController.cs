using ITI.Gymunity.FP.Application.Contracts;
using ITI.Gymunity.FP.Application.DTOs.Client;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Gymunity.FP.APIs.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class HomeClientController : ControllerBase
    {
        private readonly IHomeClientService _homeService;

        public HomeClientController(IHomeClientService homeService)
        {
            _homeService = homeService;
        }

        // GET: api/homeclient/search?term=xyz
        [HttpGet("search")]
        public async Task<ActionResult> Search([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return BadRequest("Search term is required.");

            // Search packages first (prioritize packages)
            var allPackages = await _homeService.GetAllPackagesAsync();
            var matchingPackages = allPackages
            .Where(p => (!string.IsNullOrEmpty(p.Name) && p.Name.Contains(term, StringComparison.OrdinalIgnoreCase))
            || (!string.IsNullOrEmpty(p.Description) && p.Description.Contains(term, StringComparison.OrdinalIgnoreCase)))
            .ToList();

            // Search programs and trainers
            var (programs, trainers) = await _homeService.SearchAsync(term);

            return Ok(new { packages = matchingPackages, programs, trainers });
        }

        // GET: api/homeclient/packages
        [HttpGet("packages")]
        public async Task<ActionResult<IEnumerable<PackageClientResponse>>> GetAllPackages()
        {
            var packages = await _homeService.GetAllPackagesAsync();
            return Ok(packages);
        }

        // GET: api/homeclient/packages/{id}
        [HttpGet("packages/{id:int}")]
        public async Task<ActionResult<PackageClientResponse>> GetPackageById(int id)
        {
            var pkg = await _homeService.GetPackageByIdAsync(id);
            if (pkg is null) return NotFound();
            return Ok(pkg);
        }

        // GET: api/client/homeclient/trainers
        [HttpGet("trainers")]
        public async Task<ActionResult<IEnumerable<TrainerClientResponse>>> GetAllTrainers()
        {
            var trainers = await _homeService.GetAllTrainersAsync();
            return Ok(trainers);
        }

        // GET: api/homeclient/trainers/{id}
        [HttpGet("trainers/{id:int}")]
        public async Task<ActionResult<TrainerClientResponse>> GetTrainerById(int id)
        {
            var trainer = await _homeService.GetTrainerByIdAsync(id);
            if (trainer is null) return NotFound();
            return Ok(trainer);
        }

        // GET: api/homeclient/trainers/{trainerProfileId}/packages
        [HttpGet("trainers/{trainerProfileId:int}/packages")]
        public async Task<ActionResult<IEnumerable<PackageClientResponse>>> GetPackagesByTrainerProfileId(int trainerProfileId)
        {
            var packages = await _homeService.GetPackagesByTrainerIdAsync(trainerProfileId);
            return Ok(packages);
        }

        // GET: api/homeclient/packages/byTrainerUser/{trainerUserId}
        [HttpGet("packages/byTrainerUser/{trainerUserId}")]
        public async Task<ActionResult<IEnumerable<PackageClientResponse>>> GetPackagesByTrainerUserId(int trainerUserId)
        {
            var packages = await _homeService.GetPackagesByTrainerAsync(trainerUserId);
            return Ok(packages);
        }

        // GET: api/homeclient/packages/byTrainer/{trainerProfileId}
        [HttpGet("packages/byTrainer/{trainerProfileId:int}")]
        public async Task<ActionResult<IEnumerable<PackageClientResponse>>> GetPackagesByTrainerProfileIdAlt(int trainerProfileId)
        {
            var packages = await _homeService.GetPackagesByTrainerIdAsync(trainerProfileId);
            return Ok(packages);
        }

        // --- Client program endpoints requested ---
        // GET: api/homeclient/programs
        [HttpGet("programs")]
        public async Task<ActionResult<IEnumerable<ProgramClientResponse>>> GetAllPrograms()
        {
            var programs = await _homeService.GetAllProgramsAsync();
            return Ok(programs);
        }

        // GET: api/homeclient/programs/{id}
        [HttpGet("programs/{id:int}")]
        public async Task<ActionResult<ProgramClientResponse>> GetProgramById(int id)
        {
            var program = await _homeService.GetProgramByIdAsync(id);
            if (program is null) return NotFound();
            return Ok(program);
        }

        // GET: api/homeclient/programs/byTrainer/{trainerId} (trainerId is user id)
        [HttpGet("programs/byTrainer/{trainerId}")]
        public async Task<ActionResult<IEnumerable<ProgramClientResponse>>> GetProgramsByTrainer(string trainerId)
        {
            var list = await _homeService.GetProgramsByTrainerIdAsync(trainerId);
            return Ok(list);
        }

        // New: GET: api/homeclient/programs/byTrainerUser/{trainerUserId} (user id)
        [HttpGet("programs/byTrainerUser/{trainerUserId}")]
        public async Task<ActionResult<IEnumerable<ProgramClientResponse>>> GetProgramsByTrainerUserId(string trainerUserId)
        {
            var list = await _homeService.GetProgramsByTrainerIdAsync(trainerUserId);
            return Ok(list);
        }

        // New: GET: api/homeclient/programs/byTrainerProfile/{trainerProfileId} (profile id -> maps to user id)
        [HttpGet("programs/byTrainerProfile/{trainerProfileId:int}")]
        public async Task<ActionResult<IEnumerable<ProgramClientResponse>>> GetProgramsByTrainerProfileId(int trainerProfileId)
        {
            var list = await _homeService.GetProgramsByTrainerProfileIdAsync(trainerProfileId);
            return Ok(list);
        }
    }
}
