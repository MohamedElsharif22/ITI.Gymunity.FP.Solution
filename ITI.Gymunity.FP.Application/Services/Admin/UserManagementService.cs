using ITI.Gymunity.FP.Application.Contracts.Admin;
using ITI.Gymunity.FP.Application.DTOs.Admin;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Models.Identity;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ITI.Gymunity.FP.Application.Services.Admin
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserManagementService> _logger;

        // ✅ Observable events for notification handlers
        public event Func<string, Task>? UserSuspendedAsync;
        public event Func<string, Task>? UserReactivatedAsync;
        public event Func<string, Task>? UserDeletedAsync;
        public event Func<string, UserRole, Task>? UserRoleChangedAsync;

        public UserManagementService(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<UserManagementService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<UserManagementListResponse> GetAllUsersAsync(int pageNumber = 1, int pageSize = 20)
        {
            try
            {
                var query = _userManager.Users.AsQueryable();
                var totalCount = await query.CountAsync();
                
                var users = await query
                    .OrderByDescending(u => u.CreatedAt)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var userDtos = new List<UserManagementDto>();
                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    userDtos.Add(MapToUserManagementDto(user, roles));
                }

                return new UserManagementListResponse
                {
                    Users = userDtos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all users");
                throw;
            }
        }

        public async Task<UserManagementListResponse> SearchUsersAsync(string searchTerm, int pageNumber = 1, int pageSize = 20)
        {
            try
            {
                var query = _userManager.Users
                    .Where(u => u.Email.Contains(searchTerm) ||
                               u.UserName.Contains(searchTerm) ||
                               u.FullName.Contains(searchTerm))
                    .AsQueryable();

                var totalCount = await query.CountAsync();

                var users = await query
                    .OrderByDescending(u => u.CreatedAt)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var userDtos = new List<UserManagementDto>();
                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    userDtos.Add(MapToUserManagementDto(user, roles));
                }

                return new UserManagementListResponse
                {
                    Users = userDtos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching users with term: {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<UserManagementListResponse> GetUsersByRoleAsync(UserRole role, int pageNumber = 1, int pageSize = 20)
        {
            try
            {
                var roleName = role.ToString();
                var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);

                var totalCount = usersInRole.Count;
                var users = usersInRole
                    .OrderByDescending(u => u.CreatedAt)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var userDtos = new List<UserManagementDto>();
                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    userDtos.Add(MapToUserManagementDto(user, roles));
                }

                return new UserManagementListResponse
                {
                    Users = userDtos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users by role: {Role}", role);
                throw;
            }
        }

        public async Task<UserDetailResponse> GetUserDetailAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                var roles = await _userManager.GetRolesAsync(user);

                return new UserDetailResponse
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email ?? "",
                    UserName = user.UserName ?? "",
                    PhoneNumber = user.PhoneNumber ?? "",
                    Role = user.Role,
                    ProfilePhotoUrl = user.ProfilePhotoUrl,
                    EmailConfirmed = user.EmailConfirmed,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    IsVerified = user.IsVerified,
                    IsLockedOut = user.LockoutEnd > DateTimeOffset.UtcNow,
                    LockoutEnd = user.LockoutEnd,
                    CreatedAt = user.CreatedAt,
                    LastLoginAt = user.LastLoginAt,
                    StripeCustomerId = user.StripeCustomerId,
                    StripeConnectAccountId = user.StripeConnectAccountId,
                    AccessFailedCount = user.AccessFailedCount,
                    CurrentRoles = roles
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user details for: {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> UpdateUserAsync(string userId, UpdateUserRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                if (!string.IsNullOrWhiteSpace(request.FullName))
                    user.FullName = request.FullName;

                if (!string.IsNullOrWhiteSpace(request.Email) && user.Email != request.Email)
                {
                    var emailExists = await _userManager.FindByEmailAsync(request.Email);
                    if (emailExists != null)
                        throw new Exception("Email is already in use");
                    user.Email = request.Email;
                    user.NormalizedEmail = _userManager.NormalizeName(request.Email);
                }

                if (!string.IsNullOrWhiteSpace(request.UserName) && user.UserName != request.UserName)
                {
                    var userNameExists = await _userManager.FindByNameAsync(request.UserName);
                    if (userNameExists != null)
                        throw new Exception("Username is already in use");
                    user.UserName = request.UserName;
                    user.NormalizedUserName = _userManager.NormalizeName(request.UserName);
                }

                if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
                    user.PhoneNumber = request.PhoneNumber;

                if (request.EmailConfirmed.HasValue)
                    user.EmailConfirmed = request.EmailConfirmed.Value;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to update user: {errors}");
                }

                _logger.LogInformation("User {UserId} updated successfully", userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user: {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> ChangeUserRoleAsync(string userId, UserRole newRole)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    throw new Exception("User not found");

                var currentRoles = await _userManager.GetRolesAsync(user);

                // Remove old roles
                foreach (var role in currentRoles)
                {
                    await _userManager.RemoveFromRoleAsync(user, role);
                }

                // Add new role
                var newRoleName = newRole.ToString();
                var roleExists = await _roleManager.RoleExistsAsync(newRoleName);
                if (!roleExists)
                {
                    throw new Exception($"Role {newRoleName} does not exist");
                }

                var result = await _userManager.AddToRoleAsync(user, newRoleName);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to change role: {errors}");
                }

                user.Role = newRole;
                await _userManager.UpdateAsync(user);

                _logger.LogInformation("User {UserId} role changed to {NewRole}", userId, newRole);

                // ✅ Raise event for notification handlers
                if (UserRoleChangedAsync != null)
                {
                    try
                    {
                        await UserRoleChangedAsync(userId, newRole);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Event notification failed for role change {UserId}", userId);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing user role: {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> SuspendUserAsync(string userId, string reason)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    throw new Exception("User not found");

                // Lock the account
                await _userManager.SetLockoutEnabledAsync(user, true);
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);

                _logger.LogWarning("User {UserId} suspended with reason: {Reason}", userId, reason);

                // ✅ Raise event for notification handlers
                if (UserSuspendedAsync != null)
                {
                    try
                    {
                        await UserSuspendedAsync(userId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Event notification failed for user suspension {UserId}", userId);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error suspending user: {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> ReactivateUserAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    throw new Exception("User not found");

                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                await _userManager.ResetAccessFailedCountAsync(user);

                _logger.LogInformation("User {UserId} reactivated", userId);

                // ✅ Raise event for notification handlers
                if (UserReactivatedAsync != null)
                {
                    try
                    {
                        await UserReactivatedAsync(userId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Event notification failed for user reactivation {UserId}", userId);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reactivating user: {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    throw new Exception("User not found");

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to delete user: {errors}");
                }

                _logger.LogWarning("User {UserId} deleted", userId);

                // ✅ Raise event for notification handlers
                if (UserDeletedAsync != null)
                {
                    try
                    {
                        await UserDeletedAsync(userId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Event notification failed for user deletion {UserId}", userId);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user: {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> ResetUserPasswordAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    throw new Exception("User not found");

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var tempPassword = GenerateTemporaryPassword();

                var result = await _userManager.ResetPasswordAsync(user, token, tempPassword);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to reset password: {errors}");
                }

                _logger.LogInformation("Password reset for user: {UserId}", userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting user password: {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> LockUserAccountAsync(string userId, int durationMinutes)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    throw new Exception("User not found");

                var lockoutEnd = DateTimeOffset.UtcNow.AddMinutes(durationMinutes);
                await _userManager.SetLockoutEndDateAsync(user, lockoutEnd);

                _logger.LogWarning("User {UserId} locked for {Minutes} minutes", userId, durationMinutes);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error locking user account: {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> UnlockUserAccountAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    throw new Exception("User not found");

                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);

                _logger.LogInformation("User {UserId} unlocked", userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unlocking user account: {UserId}", userId);
                throw;
            }
        }

        public async Task<IEnumerable<UserActivityLogResponse>> GetUserActivityLogsAsync(string userId, int pageSize = 50)
        {
            try
            {
                // This would typically come from an activity log table
                // For now, returning empty list
                return new List<UserActivityLogResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user activity logs for: {UserId}", userId);
                throw;
            }
        }

        public async Task<UserStatisticsResponse> GetUserStatisticsAsync()
        {
            try
            {
                var allUsers = await _userManager.Users.ToListAsync();
                var totalUsers = allUsers.Count;
                var clients = allUsers.Count(u => u.Role == UserRole.Client);
                var trainers = allUsers.Count(u => u.Role == UserRole.Trainer);
                var admins = allUsers.Count(u => u.Role == UserRole.Admin);

                return new UserStatisticsResponse
                {
                    TotalUsers = totalUsers,
                    TotalClients = clients,
                    TotalTrainers = trainers,
                    TotalAdmins = admins,
                    VerifiedTrainers = 0, // Would come from TrainerProfile table
                    UnverifiedTrainers = trainers,
                    SuspendedUsers = allUsers.Count(u => u.LockoutEnd > DateTimeOffset.UtcNow),
                    ActiveUsersLastWeek = allUsers.Count(u => u.LastLoginAt > DateTime.UtcNow.AddDays(-7)),
                    NewUsersThisMonth = allUsers.Count(u => u.CreatedAt > DateTime.UtcNow.AddMonths(-1)),
                    ClientTrainerRatio = trainers > 0 ? (double)clients / trainers : 0
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user statistics");
                throw;
            }
        }

        public async Task<IEnumerable<PendingTrainerVerificationResponse>> GetPendingTrainerVerificationsAsync()
        {
            try
            {
                // This would query TrainerProfile where IsVerified = false
                return new List<PendingTrainerVerificationResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pending trainer verifications");
                throw;
            }
        }

        public async Task<bool> VerifyTrainerAsync(string trainerId)
        {
            try
            {
                var trainer = await _userManager.FindByIdAsync(trainerId);
                if (trainer == null || trainer.Role != UserRole.Trainer)
                    throw new Exception("Trainer not found");

                trainer.IsVerified = true;
                var result = await _userManager.UpdateAsync(trainer);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to verify trainer: {errors}");
                }

                _logger.LogInformation("Trainer {TrainerId} verified", trainerId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying trainer: {TrainerId}", trainerId);
                throw;
            }
        }

        public async Task<bool> RejectTrainerVerificationAsync(string trainerId, string reason)
        {
            try
            {
                var trainer = await _userManager.FindByIdAsync(trainerId);
                if (trainer == null)
                    throw new Exception("Trainer not found");

                _logger.LogInformation("Trainer {TrainerId} verification rejected. Reason: {Reason}", trainerId, reason);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting trainer verification: {TrainerId}", trainerId);
                throw;
            }
        }

        public async Task<BulkOperationResponse> BulkUpdateUserRolesAsync(BulkRoleUpdateRequest request)
        {
            var response = new BulkOperationResponse
            {
                SuccessCount = 0,
                FailureCount = 0
            };

            try
            {
                foreach (var userId in request.UserIds)
                {
                    try
                    {
                        await ChangeUserRoleAsync(userId, request.NewRole);
                        response.SuccessCount++;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error updating role for user: {UserId}", userId);
                        response.FailureCount++;
                        response.FailedUserIds.Add(userId);
                    }
                }

                response.Message = $"Bulk role update completed. Success: {response.SuccessCount}, Failed: {response.FailureCount}";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in bulk role update operation");
                throw;
            }
        }

        public async Task<byte[]> ExportUsersAsync(string format = "csv")
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                
                if (format.ToLower() == "csv")
                {
                    return ExportToCSV(users);
                }
                
                throw new Exception("Unsupported export format");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting users");
                throw;
            }
        }

        private byte[] ExportToCSV(List<AppUser> users)
        {
            var csv = "Id,FullName,Email,UserName,Role,EmailConfirmed,IsVerified,CreatedAt,LastLoginAt\n";
            
            foreach (var user in users)
            {
                csv += $"\"{user.Id}\",\"{user.FullName}\",\"{user.Email}\",\"{user.UserName}\",\"{user.Role}\",{user.EmailConfirmed},{user.IsVerified},\"{user.CreatedAt}\",\"{user.LastLoginAt}\"\n";
            }

            return System.Text.Encoding.UTF8.GetBytes(csv);
        }

        private UserManagementDto MapToUserManagementDto(AppUser user, IList<string> roles)
        {
            return new UserManagementDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? "",
                UserName = user.UserName ?? "",
                Role = roles.FirstOrDefault() ?? user.Role.ToString(),
                EmailConfirmed = user.EmailConfirmed,
                IsVerified = user.IsVerified,
                IsLockedOut = user.LockoutEnd > DateTimeOffset.UtcNow,
                ProfilePhotoUrl = user.ProfilePhotoUrl,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                Status = user.LockoutEnd > DateTimeOffset.UtcNow ? "Suspended" : "Active"
            };
        }

        private string GenerateTemporaryPassword()
        {
            return Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        }
    }
}
