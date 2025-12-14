using ITI.Gymunity.FP.Domain.Models.Messaging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Domain.RepositoiesContracts
{
 public interface IChatRepository : IRepository<MessageThread>
 {
 Task<MessageThread?> GetByTrainerAndClientAsync(string trainerId, string clientId);
 Task<IReadOnlyList<MessageThread>> GetThreadsByTrainerAsync(string trainerId);
 Task<IReadOnlyList<Message>> GetMessagesInThreadAsync(int threadId);
 }
}
