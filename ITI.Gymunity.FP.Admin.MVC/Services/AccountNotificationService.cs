using ITI.Gymunity.FP.Infrastructure.ExternalServices;
using ITI.Gymunity.FP.Domain.Models.Enums;
using Microsoft.Extensions.Logging;

namespace ITI.Gymunity.FP.Admin.MVC.Services
{
    /// <summary>
    /// Handles user registration notifications
    /// Subscribes to events from AccountService and sends notifications to admins
    /// </summary>
    public class AccountNotificationService
    {
        private readonly IAdminNotificationService _notificationService;
        private readonly AccountService _accountService;
        private readonly AdminUserResolverService _adminUserResolver;
        private readonly ILogger<AccountNotificationService> _logger;

        public AccountNotificationService(
            IAdminNotificationService notificationService,
            AccountService accountService,
            AdminUserResolverService adminUserResolver,
            ILogger<AccountNotificationService> logger)
        {
            _notificationService = notificationService;
            _accountService = accountService;
            _adminUserResolver = adminUserResolver;
            _logger = logger;

            // ✅ Subscribe to events from AccountService
            _accountService.NewUserRegisteredAsync += OnNewUserRegisteredAsync;
            _accountService.NewGoogleUserRegisteredAsync += OnNewGoogleUserRegisteredAsync;
        }

        /// <summary>
        /// Handle new standard user registration event
        /// Creates and broadcasts notification to all admins
        /// </summary>
        private async Task OnNewUserRegisteredAsync(string userId, string fullName, string email, UserRole role)
        {
            try
            {
                var admin = await _adminUserResolver.GetPrimaryAdminAsync();
                if (admin == null)
                {
                    _logger.LogWarning("No admin user found to notify about new registration {UserId}", userId);
                    return;
                }

                var notificationType = role switch
                {
                    UserRole.Client => NotificationType.NewClientRegistration,
                    UserRole.Trainer => NotificationType.NewTrainerRegistration,
                    _ => NotificationType.SystemNotification
                };

                await _notificationService.CreateAdminNotificationAsync(
                    admin.Id,
                    $"New {role} Registration",
                    $"{fullName} ({email}) has registered as a {role}",
                    notificationType,
                    userId,
                    broadcastToAll: true);

                _logger.LogInformation("Admin notified of new user registration {UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to send new user registration notification for user {UserId}", userId);
                // Non-blocking - don't rethrow
            }
        }

        /// <summary>
        /// Handle new Google user registration event
        /// Creates and broadcasts notification to all admins
        /// </summary>
        private async Task OnNewGoogleUserRegisteredAsync(string userId, string fullName, string email, UserRole role)
        {
            try
            {
                var admin = await _adminUserResolver.GetPrimaryAdminAsync();
                if (admin == null)
                {
                    _logger.LogWarning("No admin user found to notify about new Google registration {UserId}", userId);
                    return;
                }

                var notificationType = role switch
                {
                    UserRole.Client => NotificationType.NewClientRegistration,
                    UserRole.Trainer => NotificationType.NewTrainerRegistration,
                    _ => NotificationType.SystemNotification
                };

                await _notificationService.CreateAdminNotificationAsync(
                    admin.Id,
                    $"New {role} Registration (Google Auth)",
                    $"{fullName} ({email}) has registered as a {role} using Google authentication",
                    notificationType,
                    userId,
                    broadcastToAll: true);

                _logger.LogInformation("Admin notified of new Google user registration {UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to send new Google user registration notification for user {UserId}", userId);
                // Non-blocking - don't rethrow
            }
        }
    }
}
