using ITI.Gymunity.FP.APIs.Responses;
using ITI.Gymunity.FP.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITI.Gymunity.FP.APIs.Controllers
{
    /// <summary>
    /// Controller for chatbot that answers questions about packages
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ChatBotController : BaseApiController
    {
        private readonly IChatBotService _chatBotService;
        private readonly ILogger<ChatBotController> _logger;

        public ChatBotController(IChatBotService chatBotService, ILogger<ChatBotController> logger)
        {
            _chatBotService = chatBotService;
            _logger = logger;
        }

        /// <summary>
        /// Ask a question about packages
        /// </summary>
        /// <param name="request">The question to ask</param>
        /// <returns>Answer from the chatbot</returns>
        // ai from amr start: AI endpoint starts here
        [HttpPost("ask")]
        public async Task<ActionResult<ApiResponse<ChatBotResponse>>> AskQuestion([FromBody] ChatBotRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Question))
                {
                    return BadRequest(new ApiResponse(400, "السؤال مطلوب"));
                }

                var answer = await _chatBotService.AskAboutPackagesAsync(request.Question);

                return Ok(new ApiResponse<ChatBotResponse>(new ChatBotResponse
                {
                    Answer = answer,
                    Question = request.Question
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing chatbot question");
                return StatusCode(500, new ApiResponse(500, "حدث خطأ أثناء معالجة السؤال"));
            }
        }
        // ai from amr end: AI endpoint ends here

        /// <summary>
        /// Request model for chatbot
        /// </summary>
        public class ChatBotRequest
        {
            public string Question { get; set; } = string.Empty;
        }

        /// <summary>
        /// Response model for chatbot
        /// </summary>
        public class ChatBotResponse
        {
            public string Question { get; set; } = string.Empty;
            public string Answer { get; set; } = string.Empty;
        }
    }
}

