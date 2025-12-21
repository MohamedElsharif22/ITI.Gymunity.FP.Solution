using ITI.Gymunity.FP.Domain.Models.Enums;

namespace ITI.Gymunity.FP.Application.DTOs.Client
{
 public class RegisterUserRequest
 {
 public string UserName { get; set; } = null!;
 public string Email { get; set; } = null!;
 public string FullName { get; set; } = null!;
 public string Password { get; set; } = null!;
 public UserRole Role { get; set; } = UserRole.Client;
 }
}
