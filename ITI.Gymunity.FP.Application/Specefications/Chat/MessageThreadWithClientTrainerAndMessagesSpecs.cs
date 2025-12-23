using ITI.Gymunity.FP.Domain.Models.Messaging;
using ITI.Gymunity.FP.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Specefications.Chat
{
    internal class MessageThreadWithClientTrainerAndMessagesSpecs : BaseSpecification<MessageThread>
    {
        public MessageThreadWithClientTrainerAndMessagesSpecs(int threadId) : base(mt => mt.Id == threadId)
        {
            AddInclude(mt => mt.Client);
            AddInclude(mt => mt.Trainer);
            AddInclude(mt => mt.Messages);
        }
    }
}
