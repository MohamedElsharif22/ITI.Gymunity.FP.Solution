using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.Chat;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using ITI.Gymunity.FP.Domain.Models.Messaging;
using ITI.Gymunity.FP.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Services
{
 public interface IChatService
 {
 Task<int> StartThreadAsync(StartThreadRequest request);
 Task<IReadOnlyList<MessageGetResponse>> GetThreadsForTrainerAsync(string trainerId);
 Task<MessageSendResponse> SendMessageAsync(MessageSendRequest request);
 Task<IReadOnlyList<MessageGetResponse>> GetMessagesInThreadAsync(int threadId);
 Task<bool> DeleteMessageForMeAsync(long messageId, string userId);
 Task<bool> DeleteMessageForAllAsync(long messageId);
 Task<bool> MarkThreadAsSeenAsync(int threadId, string userId);
 }

 public class ChatService : IChatService
 {
 private readonly IChatRepository _repo;
 private readonly IUnitOfWork _unitOfWork;
 private readonly IMapper _mapper;

 public ChatService(IChatRepository repo, IUnitOfWork unitOfWork, IMapper mapper)
 {
 _repo = repo;
 _unitOfWork = unitOfWork;
 _mapper = mapper;
 }

 public async Task<int> StartThreadAsync(StartThreadRequest request)
 {
 var existing = await _repo.GetByTrainerAndClientAsync(request.TrainerId, request.ClientId);
 if (existing != null) return existing.Id;
 var thread = new MessageThread { TrainerId = request.TrainerId, ClientId = request.ClientId };
 _repo.Add(thread);
 await _unitOfWork.CompleteAsync();
 if (!string.IsNullOrWhiteSpace(request.InitialMessage))
 {
 await SendMessageAsync(new MessageSendRequest { ThreadId = thread.Id, SenderId = request.TrainerId, Content = request.InitialMessage });
 }
 return thread.Id;
 }

 public async Task<IReadOnlyList<MessageGetResponse>> GetThreadsForTrainerAsync(string trainerId)
 {
 var threads = await _repo.GetThreadsByTrainerAsync(trainerId);
 return threads.SelectMany(t => t.Messages.Select(m => _mapper.Map<MessageGetResponse>(m))).ToList();
 }

 public async Task<MessageSendResponse> SendMessageAsync(MessageSendRequest request)
 {
 var message = new Message { ThreadId = request.ThreadId, SenderId = request.SenderId, Content = request.Content, MediaUrl = request.MediaUrl, Type = request.Type };
 _unitOfWork.Repository<Message>().Add(message);
 await _unitOfWork.CompleteAsync();
 return _mapper.Map<MessageSendResponse>(message);
 }

 public async Task<IReadOnlyList<MessageGetResponse>> GetMessagesInThreadAsync(int threadId)
 {
 var list = await _repo.GetMessagesInThreadAsync(threadId);
 return list.Select(m => _mapper.Map<MessageGetResponse>(m)).ToList();
 }

 public async Task<bool> DeleteMessageForMeAsync(long messageId, string userId)
 {
 var msg = await _unitOfWork.Repository<Message>().GetByIdAsync((int)messageId);
 if (msg == null) return false;
 // simple implementation: mark as IsRead = true for that user or flag deletion table (not implemented)
 msg.IsRead = true;
 _unitOfWork.Repository<Message>().Update(msg);
 await _unitOfWork.CompleteAsync();
 return true;
 }

 public async Task<bool> DeleteMessageForAllAsync(long messageId)
 {
 var msg = await _unitOfWork.Repository<Message>().GetByIdAsync((int)messageId);
 if (msg == null) return false;
 _unitOfWork.Repository<Message>().Delete(msg);
 await _unitOfWork.CompleteAsync();
 return true;
 }

 public async Task<bool> MarkThreadAsSeenAsync(int threadId, string userId)
 {
 var msgs = await _repo.GetMessagesInThreadAsync(threadId);
 var toUpdate = msgs.Where(m => !m.IsRead && m.SenderId != userId).ToList();
 foreach (var m in toUpdate)
 {
 m.IsRead = true;
 _unitOfWork.Repository<Message>().Update(m);
 }
 await _unitOfWork.CompleteAsync();
 return true;
 }
 }
}
