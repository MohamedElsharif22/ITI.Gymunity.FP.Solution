namespace ITI.Gymunity.FP.Application.DTOs.Chat
{
 public class StartThreadRequest
 {
 public string TrainerId { get; set; } = null!;
 public string ClientId { get; set; } = null!;
 public string InitialMessage { get; set; } = string.Empty;
 }
}
