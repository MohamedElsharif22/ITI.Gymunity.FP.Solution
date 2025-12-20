using ITI.Gymunity.FP.Infrastructure.DTOs.Notifications;

namespace ITI.Gymunity.FP.Infrastructure.Contracts.Services
{
    public interface INotificationService
    {
        Task<NotificationResponse> CreateNotificationAsync(string userId, string title, string message, 
            int type, string? relatedEntityId = null);
        Task<IEnumerable<NotificationResponse>> GetUserNotificationsAsync(string userId, bool unreadOnly = false);
        Task<bool> MarkNotificationAsReadAsync(int notificationId);
        Task<bool> MarkAllNotificationsAsReadAsync(string userId);
        Task<int> GetUnreadNotificationCountAsync(string userId);
    }
}
