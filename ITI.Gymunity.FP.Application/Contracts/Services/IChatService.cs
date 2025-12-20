using ITI.Gymunity.FP.Infrastructure.DTOs.Messaging;

namespace ITI.Gymunity.FP.Infrastructure.Contracts.Services
{
    public interface IChatService
    {
        Task<MessageResponse> SendMessageAsync(int threadId, string senderId, SendMessageRequest request);
        Task<IEnumerable<MessageResponse>> GetMessageThreadAsync(int threadId);
        Task<IEnumerable<object>> GetUserChatsAsync(string userId);
        Task<bool> MarkMessageAsReadAsync(long messageId);
        Task<bool> MarkThreadAsReadAsync(int threadId, string userId);
    }
}
