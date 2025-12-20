using ITI.Gymunity.FP.Domain.Models.Messaging;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using ITI.Gymunity.FP.Infrastructure._Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Infrastructure.Repositories
{
 public class ChatRepository : Repository<MessageThread>, IChatRepository
 {
 public ChatRepository(AppDbContext context) : base(context) { }

 public async Task<MessageThread?> GetByTrainerAndClientAsync(string trainerId, string clientId)
 {
 return await _Context.MessageThreads
 .Include(t => t.Messages)
 .ThenInclude(m => m.Sender)
 .FirstOrDefaultAsync(t => t.TrainerId == trainerId && t.ClientId == clientId);
 }

 public async Task<IReadOnlyList<MessageThread>> GetThreadsByTrainerAsync(string trainerId)
 {
 return await _Context.MessageThreads
 .Where(t => t.TrainerId == trainerId)
 .Include(t => t.Messages)
 .ThenInclude(m => m.Sender)
 .ToListAsync();
 }

 public async Task<IReadOnlyList<Message>> GetMessagesInThreadAsync(int threadId)
 {
 return await _Context.Messages
 .Where(m => m.ThreadId == threadId)
 .Include(m => m.Sender)
 .OrderBy(m => m.CreatedAt)
 .ToListAsync();
 }
 }
}
