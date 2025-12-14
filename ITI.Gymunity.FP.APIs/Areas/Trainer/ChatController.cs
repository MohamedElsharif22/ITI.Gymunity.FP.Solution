using ITI.Gymunity.FP.Application.DTOs.Chat;
using ITI.Gymunity.FP.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Gymunity.FP.APIs.Areas.Trainer
{
 [Area("Trainer")]
 [Route("api/trainer/chat")]
 [ApiController]
 public class ChatController : TrainerBaseController
 {
 private readonly IChatService _service;

 public ChatController(IChatService service)
 {
 _service = service;
 }

 [HttpPost("threads/start")]
 public async Task<IActionResult> StartThread([FromBody] StartThreadRequest request)
 {
 var id = await _service.StartThreadAsync(request);
 return Ok(new { threadId = id });
 }

 [HttpGet("threads/{trainerId}")]
 public async Task<IActionResult> GetThreads(string trainerId)
 {
 var list = await _service.GetThreadsForTrainerAsync(trainerId);
 return Ok(list);
 }

 [HttpPost("messages/send")]
 public async Task<IActionResult> SendMessage([FromBody] MessageSendRequest request)
 {
 var res = await _service.SendMessageAsync(request);
 return Ok(res);
 }

 [HttpGet("messages/thread/{threadId:int}")]
 public async Task<IActionResult> GetMessagesInThread(int threadId)
 {
 var list = await _service.GetMessagesInThreadAsync(threadId);
 return Ok(list);
 }

 [HttpDelete("messages/{messageId}/delete-for-me")]
 public async Task<IActionResult> DeleteForMe(long messageId, [FromQuery] string userId)
 {
 var ok = await _service.DeleteMessageForMeAsync(messageId, userId);
 if (!ok) return NotFound();
 return NoContent();
 }

 [HttpDelete("messages/{messageId}/delete-for-all")]
 public async Task<IActionResult> DeleteForAll(long messageId)
 {
 var ok = await _service.DeleteMessageForAllAsync(messageId);
 if (!ok) return NotFound();
 return NoContent();
 }

 [HttpPost("threads/{threadId}/seen")]
 public async Task<IActionResult> MarkSeen(int threadId, [FromBody] string userId)
 {
 var ok = await _service.MarkThreadAsSeenAsync(threadId, userId);
 if (!ok) return NotFound();
 return NoContent();
 }
 }
}
