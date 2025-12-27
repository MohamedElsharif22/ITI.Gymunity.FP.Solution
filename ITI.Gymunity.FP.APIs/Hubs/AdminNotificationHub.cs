using ITI.Gymunity.FP.Application.Contracts.ExternalServices;
using ITI.Gymunity.FP.Application.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ITI.Gymunity.FP.APIs.Hubs
{
    /// <summary>
    /// SignalR Hub for admin-specific real-time notifications and communications
    /// </summary>
    [Authorize]
    public class AdminNotificationHub(
        INotificationService notificationService,
        ISignalRConnectionManager connectionManager,
        ILogger<AdminNotificationHub> logger) : Hub
    {
        private readonly INotificationService _notificationService = notificationService;
        private readonly ISignalRConnectionManager _connectionManager = connectionManager;
        private readonly ILogger<AdminNotificationHub> _logger = logger;

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                _connectionManager.AddConnection(userId, Context.ConnectionId);
                _logger.LogInformation($"Admin {userId} connected to admin notification hub with connection ID {Context.ConnectionId}");
                
                // Notify admins that a new admin is online
                await Clients.All.SendAsync("AdminOnline", userId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _connectionManager.RemoveConnection(Context.ConnectionId);
            
            if (!string.IsNullOrEmpty(userId))
            {
                _logger.LogInformation($"Admin {userId} disconnected from admin notification hub");
                
                // Notify admins that an admin is offline
                var remainingConnections = _connectionManager.GetAllConnections(userId);
                if (!remainingConnections.Any())
                {
                    await Clients.All.SendAsync("AdminOffline", userId);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Get all notifications for admin
        /// </summary>
        public async Task GetAdminNotifications()
        {
            try
            {
                var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    await Clients.Caller.SendAsync("Error", "User not authenticated");
                    return;
                }

                var notifications = await _notificationService.GetUserNotificationsAsync(userId);
                await Clients.Caller.SendAsync("AdminNotifications", notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching admin notifications: {ex.Message}");
                await Clients.Caller.SendAsync("Error", ex.Message);
            }
        }

        /// <summary>
        /// Get unread count
        /// </summary>
        public async Task GetUnreadCount()
        {
            try
            {
                var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    await Clients.Caller.SendAsync("Error", "User not authenticated");
                    return;
                }

                var count = await _notificationService.GetUnreadNotificationCountAsync(userId);
                await Clients.Caller.SendAsync("UnreadCount", count);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching unread count: {ex.Message}");
                await Clients.Caller.SendAsync("Error", ex.Message);
            }
        }

        /// <summary>
        /// Broadcast notification to all admins
        /// </summary>
        public async Task BroadcastAdminNotification(string title, string message, int type)
        {
            try
            {
                var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    await Clients.Caller.SendAsync("Error", "User not authenticated");
                    return;
                }

                // Broadcast to all connected admins
                await Clients.All.SendAsync("NotificationBroadcast", new
                {
                    title,
                    message,
                    type,
                    timestamp = DateTime.UtcNow
                });

                _logger.LogInformation($"Admin notification broadcasted by {userId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error broadcasting notification: {ex.Message}");
                await Clients.Caller.SendAsync("Error", ex.Message);
            }
        }

        /// <summary>
        /// Send urgent alert to specific admin
        /// </summary>
        public async Task SendUrgentAlert(string targetAdminId, string title, string message)
        {
            try
            {
                var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    await Clients.Caller.SendAsync("Error", "User not authenticated");
                    return;
                }

                await Clients.User(targetAdminId).SendAsync("UrgentAlert", new
                {
                    title,
                    message,
                    timestamp = DateTime.UtcNow,
                    fromAdmin = userId
                });

                _logger.LogInformation($"Urgent alert sent to {targetAdminId} by {userId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending urgent alert: {ex.Message}");
                await Clients.Caller.SendAsync("Error", ex.Message);
            }
        }

        /// <summary>
        /// Mark notification as read
        /// </summary>
        public async Task MarkAsRead(int notificationId)
        {
            try
            {
                await _notificationService.MarkNotificationAsReadAsync(notificationId);
                await Clients.All.SendAsync("NotificationRead", notificationId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error marking notification as read: {ex.Message}");
                await Clients.Caller.SendAsync("Error", ex.Message);
            }
        }
    }
}
