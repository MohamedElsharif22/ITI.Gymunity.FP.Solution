using AutoMapper;
using ITI.Gymunity.FP.Application.Common;
using ITI.Gymunity.FP.Application.Specefications.Subscriptions;
using ITI.Gymunity.FP.Application.Specefications.Trainer;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using Microsoft.Extensions.Logging;
using ITI.Gymunity.FP.Application.DTOs.User.Subscribe;

namespace ITI.Gymunity.FP.Application.Services
{
    public class SubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<SubscriptionService> _logger;

        public SubscriptionService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<SubscriptionService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Subscribe to a trainer's package
        /// </summary>
        public async Task<ServiceResult<SubscriptionResponse>> SubscribeAsync(
            string clientId,
            SubscribePackageRequest request)
        {
            try
            {
                // 1. Validate Package exists
                var package = await _unitOfWork
                    .Repository<Package>()
                    .GetByIdAsync(request.PackageId);

                if (package == null)
                    return ServiceResult<SubscriptionResponse>.Failure(
                        $"Package with ID {request.PackageId} not found");

                // 2. Check if already subscribed to this package
                var existingSubSpec = new ActiveClientSubscriptionForPackageSpecs(
                    clientId,
                    request.PackageId);
                var existingSub = await _unitOfWork
                    .Repository<Subscription>()
                    .GetWithSpecsAsync(existingSubSpec);

                if (existingSub != null)
                    return ServiceResult<SubscriptionResponse>.Failure(
                        "You are already subscribed to this package");

                //// 3. Validate Trainer is verified and active
                //var trainerSpec = new TrainerByUserIdSpecs(package.TrainerId);
                //var trainer = await _unitOfWork
                //    .Repository<TrainerProfile>()
                //    .GetWithSpecsAsync(trainerSpec);

                //if (trainer == null || !trainer.IsVerified)
                //    return ServiceResult<SubscriptionResponse>.Failure(
                //        "Trainer is not available");

                // 4. Calculate amount and period
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

                // 6. Map and return response
                var response = _mapper.Map<SubscriptionResponse>(subscription);

                _logger.LogInformation(
                    "Client {ClientId} successfully subscribed to package {PackageId}",
                    clientId,
                    request.PackageId);

                return ServiceResult<SubscriptionResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error subscribing client {ClientId} to package {PackageId}",
                    clientId,
                    request.PackageId);

                return ServiceResult<SubscriptionResponse>.Failure(
                    "An error occurred while processing your subscription. Please try again later.");
            }
        }

        /// <summary>
        /// Get all client's subscriptions with optional status filter
        /// </summary>
        public async Task<ServiceResult<IEnumerable<SubscriptionResponse>>> GetMySubscriptionsAsync(
            string clientId,
            SubscriptionStatus? status = null)
        {
            try
            {
                var spec = new ClientSubscriptionsSpecs(clientId, status);
                var subs = await _unitOfWork
                    .Repository<Subscription>()
                    .GetAllWithSpecsAsync(spec);

                var result = subs.Select(s => _mapper.Map<SubscriptionResponse>(s));

                return ServiceResult<IEnumerable<SubscriptionResponse>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error fetching subscriptions for client {ClientId}",
                    clientId);

                return ServiceResult<IEnumerable<SubscriptionResponse>>.Failure(
                    "An error occurred while fetching your subscriptions.");
            }
        }

        /// <summary>
        /// Get single subscription by ID for current client
        /// </summary>
        public async Task<ServiceResult<SubscriptionResponse>> GetSubscriptionByIdAsync(
            int id,
            string clientId)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error fetching subscription {SubscriptionId} for client {ClientId}",
                    id,
                    clientId);

                return ServiceResult<SubscriptionResponse>.Failure(
                    "An error occurred while fetching the subscription.");
            }
        }

        /// <summary>
        /// Cancel subscription (client keeps access until period end)
        /// </summary>
        public async Task<ServiceResult<bool>> CancelAsync(int id, string clientId)
        {
            try
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

                // Client keeps access until CurrentPeriodEnd (per SRS UC-10)
                sub.Status = SubscriptionStatus.Canceled;
                sub.CanceledAt = DateTime.UtcNow;

                await _unitOfWork.CompleteAsync();

                _logger.LogInformation(
                    "Client {ClientId} canceled subscription {SubscriptionId}",
                    clientId,
                    id);

                return ServiceResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error canceling subscription {SubscriptionId} for client {ClientId}",
                    id,
                    clientId);

                return ServiceResult<bool>.Failure(
                    "An error occurred while canceling the subscription.");
            }
        }

        /// <summary>
        /// Reactivate a canceled subscription (if not expired)
        /// </summary>
        public async Task<ServiceResult<bool>> ReactivateAsync(int id, string clientId)
        {
            try
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

                _logger.LogInformation(
                    "Client {ClientId} reactivated subscription {SubscriptionId}",
                    clientId,
                    id);

                return ServiceResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error reactivating subscription {SubscriptionId} for client {ClientId}",
                    id,
                    clientId);

                return ServiceResult<bool>.Failure(
                    "An error occurred while reactivating the subscription.");
            }
        }

        /// <summary>
        /// Check if client has active subscription to a specific trainer
        /// </summary>
        public async Task<bool> HasActiveSubscriptionToTrainerAsync(
            string clientId,
            string trainerId)
        {
            try
            {
                var spec = new ActiveClientSubscriptionToTrainerSpecs(clientId, trainerId);
                var sub = await _unitOfWork
                    .Repository<Subscription>()
                    .GetWithSpecsAsync(spec);

                return sub != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error checking trainer access for client {ClientId} and trainer {TrainerId}",
                    clientId,
                    trainerId);


                return false;
            }
        }
    }
}