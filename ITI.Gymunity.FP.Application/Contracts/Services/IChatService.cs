using ITI.Gymunity.FP.Application.DTOs.Messaging;

namespace ITI.Gymunity.FP.Application.Contracts.Services
{
    public interface IChatService
    {
        Task<MessageResponse> SendMessageAsync(int threadId, string senderId, SendMessageRequest request);
        Task<IEnumerable<MessageResponse>> GetMessageThreadAsync(int threadId);
        Task<IEnumerable<object>> GetUserChatsAsync(string userId);
        Task<bool> MarkMessageAsReadAsync(long messageId);
        Task<bool> MarkThreadAsReadAsync(int threadId, string userId);
        Task<CreateChatThreadResponse> CreateChatThreadAsync(string clientId, string trainerId);
    }
}
