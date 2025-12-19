using ITI.Gymunity.FP.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Gymunity.FP.APIs.Areas.Trainer
{
 public class ClientsController : TrainerBaseController
 {
 private readonly IClientService _service;

 public ClientsController(IClientService service)
 {
 _service = service;
 }

 [HttpGet]
 public async Task<IActionResult> GetAllClients(string trainerId)
 {
 var list = await _service.GetAllByTrainerIdAsync(trainerId);
 return Ok(list);
 }
 }
}
