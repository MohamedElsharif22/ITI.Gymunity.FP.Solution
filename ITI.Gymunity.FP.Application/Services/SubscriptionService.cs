using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.User.Subscribe;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Enums;

public class SubscriptionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SubscriptionService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // 🔵 Subscribe
    public async Task<SubscriptionResponse> SubscribeAsync(string clientId, SubscribePackageRequest request)
    {
        var subscription = new Subscription
        {
            ClientId = clientId,
            PackageId = request.PackageId,
            AmountPaid = 0, // placeholder
            CurrentPeriodEnd = DateTime.UtcNow.AddMonths(1)
        };

        _unitOfWork.Repository<Subscription>().Add(subscription);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<SubscriptionResponse>(subscription);
    }

    // 🔵 Get all my subscriptions
    public async Task<IEnumerable<SubscriptionResponse>> GetMySubscriptionsAsync(string clientId)
    {
        var spec = new ClientSubscriptionsSpecs(clientId);

        var subs = await _unitOfWork
            .Repository<Subscription>()
            .GetAllWithSpecsAsync(spec);

        return subs.Select(s => _mapper.Map<SubscriptionResponse>(s));
    }

    // 🔵 Cancel
    public async Task CancelAsync(int id, string clientId)
    {
        var spec = new ClientSubscriptionByIdSpecs(id, clientId);

        var sub = await _unitOfWork
            .Repository<Subscription>()
            .GetWithSpecsAsync(spec);

        if (sub == null) throw new Exception("Subscription not found");

        sub.Status = SubscriptionStatus.Canceled;
        sub.CanceledAt = DateTime.UtcNow;

        await _unitOfWork.CompleteAsync();
    }
}
