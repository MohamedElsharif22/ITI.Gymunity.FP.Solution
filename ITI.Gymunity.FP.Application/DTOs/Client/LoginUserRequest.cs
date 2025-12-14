namespace ITI.Gymunity.FP.Application.DTOs.Client
{
 public class LoginUserRequest
 {
 public string? UserName { get; set; }
 public string? Email { get; set; }
 public string Password { get; set; } = null!;
 }
}
