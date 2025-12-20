using ITI.Gymunity.FP.Domain.Models.Messaging;
using ITI.Gymunity.FP.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Specefications.Chat
{
    internal class MessageWithSenderAndTreadSpecs : BaseSpecification<Message>
    {
        public MessageWithSenderAndTreadSpecs(string senderId) : base(m => m.SenderId == senderId)
        {
            AddInclude(m => m.Sender);
            AddInclude(m => m.Thread);
        }
    }
}
