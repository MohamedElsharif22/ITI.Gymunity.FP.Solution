using ITI.Gymunity.FP.Domain.Models.Messaging;
using ITI.Gymunity.FP.Domain.Models.Enums;
using System;

namespace ITI.Gymunity.FP.Application.DTOs.Chat
{
 public class MessageSendRequest
 {
 public int ThreadId { get; set; }
 public string SenderId { get; set; } = null!;
 public string Content { get; set; } = string.Empty;
 public string? MediaUrl { get; set; }
 public MessageType Type { get; set; } = MessageType.Text;
 }
}
