using System;
using ITI.Gymunity.FP.Domain.Models.Messaging;
using ITI.Gymunity.FP.Domain.Models.Enums;

namespace ITI.Gymunity.FP.Application.DTOs.Chat
{
 public class MessageGetResponse
 {
 public long Id { get; set; }
 public int ThreadId { get; set; }
 public string SenderId { get; set; } = null!;
 public string Content { get; set; } = string.Empty;
 public string? MediaUrl { get; set; }
 public MessageType Type { get; set; }
 public DateTime CreatedAt { get; set; }
 public bool IsRead { get; set; }
 public MessageReadStatus ReadStatus { get; set; }
 }
}
