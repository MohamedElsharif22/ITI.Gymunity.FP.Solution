using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace ITI.Gymunity.FP.APIs.Areas.Trainer
{
    [ApiController]
    [Route("api/trainer/[controller]")]
    public class PackagesController : TrainerBaseController
    {
        private readonly IPackageService _service;

        public PackagesController(IPackageService service)
        {
            _service = service;
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
        public async Task<IActionResult> GetByTrainer(string trainerId)
        {
            if (string.IsNullOrWhiteSpace(trainerId))
                return BadRequest("trainerId is required.");

            var list = await _service.GetAllForTrainerAsync(trainerId);
            return Ok(list);
        }

        // ============================
        // POST: api/trainer/Packages
        // Guest (temporary): Create package
        // trainerId comes from request body
        // ============================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PackageCreateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.TrainerId))
                return BadRequest("TrainerId is required.");

            var created = await _service.CreateAsync(request.TrainerId, request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                created
            );
        }

        // ============================
        // PUT: api/trainer/Packages/{id}
        // Guest (temporary): Update package
        // ============================
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PackageCreateRequest request)
        {
            var updated = await _service.UpdateAsync(id, request);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        // ============================
        // DELETE: api/trainer/Packages/{id}
        // Guest (temporary): Delete package
        // ============================
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound();

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
    }
}


//?? „·«ÕŸ«  „Â„… Ãœ«
//?? ·ÌÂ œÂ ÂÌ‘ €· ··‹ Guestø

//? „›Ì‘:

//User

//Claims

//Unauthorized()

//[Authorize]

//?? ﬂ·Â »Ì⁄ „œ ⁄·Ï Parameters ’—ÌÕ…

//?? „·«ÕŸ… √„‰Ì… („Â„…)

//«··Ì ›Êﬁ œÂ Temporary / Development Only
//›Ì «·≈‰ «Ã «·’Õ ÌﬂÊ‰:

//Guest:

//GET

//Trainer:

//POST / PUT / DELETE

//Ê  Ÿ»ÿ »‹:

//[Authorize(Roles = "Trainer")]
