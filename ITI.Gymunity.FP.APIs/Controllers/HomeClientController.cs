using ITI.Gymunity.FP.Application.DTOs.Client;
using ITI.Gymunity.FP.Application.Specefications;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.APIs.Controllers
{
 
 [Route("api/[controller]")]
 [ApiController]
 public class HomeClientController : ControllerBase
 {
 private readonly ITI.Gymunity.FP.Application.Services.IHomeClientService _homeService;

 public HomeClientController(ITI.Gymunity.FP.Application.Services.IHomeClientService homeService)
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
 }
}
