using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Domain.Models.Messaging
{
    public class Message : BaseEntity
    {
        public new long Id { get; set; }
        public int ThreadId { get; set; }
        public string SenderId { get; set; } = null!;
        public string Content { get; set; } = string.Empty;
        public string? MediaUrl { get; set; } // photo/video/voice note
        public MessageType Type { get; set; } = MessageType.Text;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;

        public AppUser Sender { get; set; } = null!;
        public MessageThread Thread { get; set; } = null!;
    }
}
