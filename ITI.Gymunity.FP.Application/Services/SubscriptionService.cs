using AutoMapper;
using ITI.Gymunity.FP.Application.Common;
using ITI.Gymunity.FP.Application.DTOs.User.Subscribe;
using ITI.Gymunity.FP.Application.Specefications.Subscription;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Models.Trainer;

namespace ITI.Gymunity.FP.Application.Services
{
    public class SubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubscriptionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult<SubscriptionResponse>> SubscribeAsync(
            string clientId,
            SubscribePackageRequest request)
        {
            // 1. Validate Package
            var package = await _unitOfWork
                .Repository<Package>()
                .GetByIdAsync(request.PackageId);

            if (package == null)
                return ServiceResult<SubscriptionResponse>.Failure(
                    $"Package with ID {request.PackageId} not found");

            // 2. Check existing subscription
            var existingSubSpec = new ActiveClientSubscriptionForPackageSpecs(
                clientId,
                request.PackageId);
            var existingSub = await _unitOfWork
                .Repository<Subscription>()
                .GetWithSpecsAsync(existingSubSpec);

            if (existingSub != null)
                return ServiceResult<SubscriptionResponse>.Failure(
                    "You are already subscribed to this package");

         
            // 4. Calculate amount
            decimal amount = request.IsAnnual
                ? package.PriceYearly ?? package.PriceMonthly * 12
                : package.PriceMonthly;

            DateTime periodEnd = request.IsAnnual
                ? DateTime.UtcNow.AddYears(1)
                : DateTime.UtcNow.AddMonths(1);

            // 5. Create subscription
            var subscription = new Subscription
            {
                ClientId = clientId,
                PackageId = request.PackageId,
                Status = SubscriptionStatus.Unpaid,  
                StartDate = DateTime.UtcNow,
                CurrentPeriodEnd = periodEnd,
                AmountPaid = amount,
                Currency = "EGP",
                PlatformFeePercentage = 15m
            };

            _unitOfWork.Repository<Subscription>().Add(subscription);
            await _unitOfWork.CompleteAsync();

            var response = _mapper.Map<SubscriptionResponse>(subscription);
            return ServiceResult<SubscriptionResponse>.Success(response);
        }

        public async Task<ServiceResult<IEnumerable<SubscriptionResponse>>> GetMySubscriptionsAsync(
            string clientId,
            SubscriptionStatus? status = null)
        {
            var spec = new ClientSubscriptionsSpecs(clientId, status);
            var subs = await _unitOfWork
                .Repository<Subscription>()
                .GetAllWithSpecsAsync(spec);

            var result = subs.Select(s => _mapper.Map<SubscriptionResponse>(s));
            return ServiceResult<IEnumerable<SubscriptionResponse>>.Success(result);
        }

        public async Task<ServiceResult<SubscriptionResponse>> GetSubscriptionByIdAsync(
            int id,
            string clientId)
        {
            var spec = new ClientSubscriptionByIdSpecs(id, clientId);
            var sub = await _unitOfWork
                .Repository<Subscription>()
                .GetWithSpecsAsync(spec);

            if (sub == null)
                return ServiceResult<SubscriptionResponse>.Failure(
                    "Subscription not found");

            var response = _mapper.Map<SubscriptionResponse>(sub);
            return ServiceResult<SubscriptionResponse>.Success(response);
        }

        public async Task<ServiceResult<bool>> CancelAsync(int id, string clientId)
        {
            var spec = new ClientSubscriptionByIdSpecs(id, clientId);
            var sub = await _unitOfWork
                .Repository<Subscription>()
                .GetWithSpecsAsync(spec);

            if (sub == null)
                return ServiceResult<bool>.Failure("Subscription not found");

            if (sub.Status == SubscriptionStatus.Canceled)
                return ServiceResult<bool>.Failure(
                    "Subscription is already canceled");

            sub.Status = SubscriptionStatus.Canceled;
            sub.CanceledAt = DateTime.UtcNow;

            await _unitOfWork.CompleteAsync();

            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<bool>> ReactivateAsync(int id, string clientId)
        {
            var spec = new ClientSubscriptionByIdSpecs(id, clientId);
            var sub = await _unitOfWork
                .Repository<Subscription>()
                .GetWithSpecsAsync(spec);

            if (sub == null)
                return ServiceResult<bool>.Failure("Subscription not found");

            if (sub.Status != SubscriptionStatus.Canceled)
                return ServiceResult<bool>.Failure(
                    "Only canceled subscriptions can be reactivated");

            if (DateTime.UtcNow > sub.CurrentPeriodEnd)
                return ServiceResult<bool>.Failure(
                    "Subscription period has ended. Please subscribe again.");

            sub.Status = SubscriptionStatus.Active;
            sub.CanceledAt = null;

            await _unitOfWork.CompleteAsync();

            return ServiceResult<bool>.Success(true);
        }

        public async Task<bool> HasActiveSubscriptionToTrainerAsync(
            string clientId,
            string trainerId)
        {
            var spec = new ActiveClientSubscriptionToTrainerSpecs(clientId, trainerId);
            var sub = await _unitOfWork
                .Repository<Subscription>()
                .GetWithSpecsAsync(spec);

            return sub != null;
        }
    }
}