using ITI.Gymunity.FP.APIs.Responses;
using ITI.Gymunity.FP.Application.Contracts.Services;
using ITI.Gymunity.FP.Application.DTOs.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITI.Gymunity.FP.APIs.Controllers
{
    /// <summary>
    /// Provides API endpoints for managing chat threads and messages for authenticated users.
    /// </summary>
    /// <remarks>All actions in this controller require the user to be authenticated. The controller enables
    /// users to retrieve their chat threads, view messages within a thread, send messages, and mark messages or entire
    /// threads as read. Most endpoints return results in a standard response format indicating success or failure. For
    /// real-time messaging, consider using SignalR in addition to these endpoints.</remarks>
    [Authorize]
    public class ChatController(IChatService chatService, ILogger<ChatController> logger) : BaseApiController
    {
        private readonly IChatService _chatService = chatService;
        private readonly ILogger<ChatController> _logger = logger;

        private string? GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier);
        /// <summary>
        /// Get all chat threads for the current user
        /// </summary>
        [HttpGet("threads")]
        public async Task<ActionResult<ApiResponse<IEnumerable<object?>>>> GetUserChats()
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new ApiResponse(401));

                var chats = await _chatService.GetUserChatsAsync(userId);
                return Ok(new { success = true, data = chats });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching user chats: {ex.Message}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Get messages for a specific thread
        /// </summary>
        [HttpGet("threads/{threadId}/messages")]
        public async Task<ActionResult> GetThreadMessages(int threadId)
        {
            try
            {
                var messages = await _chatService.GetMessageThreadAsync(threadId);
                return Ok(new { success = true, data = messages });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching thread messages: {ex.Message}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Send a message to a thread (can also be done via SignalR for real-time)
        /// </summary>
        [HttpPost("threads/{threadId}/messages")]
        public async Task<ActionResult> SendMessage(int threadId, [FromBody] SendMessageRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var message = await _chatService.SendMessageAsync(threadId, userId, request);
                return Ok(new ApiResponse<MessageResponse>(message));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending message: {ex.Message}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Mark a message as read
        /// </summary>
        [HttpPut("messages/{messageId}/read")]
        public async Task<ActionResult> MarkMessageAsRead(long messageId)
        {
            try
            {
                var result = await _chatService.MarkMessageAsReadAsync(messageId);
                if (!result)
                    return NotFound(new { success = false, message = "Message not found" });

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error marking message as read: {ex.Message}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Mark entire thread as read
        /// </summary>
        [HttpPut("threads/{threadId}/read")]
        public async Task<ActionResult> MarkThreadAsRead(int threadId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var result = await _chatService.MarkThreadAsReadAsync(threadId, userId);
                if (!result)
                    return NotFound(new { success = false, message = "Thread not found" });

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error marking thread as read: {ex.Message}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
