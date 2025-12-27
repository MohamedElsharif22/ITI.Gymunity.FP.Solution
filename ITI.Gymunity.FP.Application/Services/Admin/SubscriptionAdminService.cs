using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.User.Subscribe;
using ITI.Gymunity.FP.Application.Specefications.Admin;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Services.Admin
{
    /// <summary>
    /// Admin service for managing subscriptions and subscription lifecycle
    /// </summary>
    public class SubscriptionAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<SubscriptionAdminService> _logger;

        public SubscriptionAdminService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<SubscriptionAdminService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all subscriptions with optional filtering and pagination
        /// </summary>
        public async Task<IEnumerable<SubscriptionResponse>> GetAllSubscriptionsAsync(
            SubscriptionFilterSpecs specs)
        {
            try
            {
                var subscriptions = await _unitOfWork
                    .Repository<Subscription>()
                    .GetAllWithSpecsAsync(specs);

                return _mapper.Map<IEnumerable<SubscriptionResponse>>(subscriptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving subscriptions with specs");
                throw;
            }
        }

        /// <summary>
        /// Get subscription by ID
        /// </summary>
        public async Task<SubscriptionResponse?> GetSubscriptionByIdAsync(int subscriptionId)
        {
            try
            {
                var subscription = await _unitOfWork
                    .Repository<Subscription>()
                    .GetByIdAsync(subscriptionId);

                if (subscription == null)
                    return null;

                return _mapper.Map<SubscriptionResponse>(subscription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving subscription with ID {SubscriptionId}", subscriptionId);
                throw;
            }
        }

        /// <summary>
        /// Get count of subscriptions matching specification
        /// </summary>
        public async Task<int> GetSubscriptionCountAsync(SubscriptionFilterSpecs specs)
        {
            try
            {
                return await _unitOfWork
                    .Repository<Subscription>()
                    .GetCountWithspecsAsync(specs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting subscription count");
                throw;
            }
        }

        /// <summary>
        /// Get active subscriptions
        /// </summary>
        public async Task<IEnumerable<SubscriptionResponse>> GetActiveSubscriptionsAsync(
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var specs = new SubscriptionFilterSpecs(
                    status: SubscriptionStatus.Active,
                    pageNumber: pageNumber,
                    pageSize: pageSize);

                var subscriptions = await _unitOfWork
                    .Repository<Subscription>()
                    .GetAllWithSpecsAsync(specs);

                return _mapper.Map<IEnumerable<SubscriptionResponse>>(subscriptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active subscriptions");
                throw;
            }
        }

        /// <summary>
        /// Get inactive subscriptions
        /// </summary>
        public async Task<IEnumerable<SubscriptionResponse>> GetInactiveSubscriptionsAsync(
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var specs = new SubscriptionFilterSpecs(
                    status: SubscriptionStatus.Canceled,
                    pageNumber: pageNumber,
                    pageSize: pageSize);

                var subscriptions = await _unitOfWork
                    .Repository<Subscription>()
                    .GetAllWithSpecsAsync(specs);

                return _mapper.Map<IEnumerable<SubscriptionResponse>>(subscriptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving inactive subscriptions");
                throw;
            }
        }

        /// <summary>
        /// Get unpaid subscriptions
        /// </summary>
        public async Task<IEnumerable<SubscriptionResponse>> GetUnpaidSubscriptionsAsync(
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var specs = new SubscriptionFilterSpecs(
                    status: SubscriptionStatus.Unpaid,
                    pageNumber: pageNumber,
                    pageSize: pageSize);

                var subscriptions = await _unitOfWork
                    .Repository<Subscription>()
                    .GetAllWithSpecsAsync(specs);

                return _mapper.Map<IEnumerable<SubscriptionResponse>>(subscriptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving unpaid subscriptions");
                throw;
            }
        }

        /// <summary>
        /// Cancel subscription (admin action)
        /// </summary>
        public async Task<bool> CancelSubscriptionAsync(int subscriptionId, string reason = "")
        {
            try
            {
                var subscription = await _unitOfWork
                    .Repository<Subscription>()
                    .GetByIdAsync(subscriptionId);

                if (subscription == null)
                {
                    _logger.LogWarning("Subscription with ID {SubscriptionId} not found for cancellation", subscriptionId);
                    return false;
                }

                if (subscription.Status == SubscriptionStatus.Canceled)
                {
                    _logger.LogWarning("Subscription {SubscriptionId} already canceled", subscriptionId);
                    return false;
                }

                subscription.Status = SubscriptionStatus.Canceled;
                subscription.CanceledAt = DateTime.UtcNow;

                _unitOfWork.Repository<Subscription>().Update(subscription);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("Subscription {SubscriptionId} canceled by admin. Reason: {Reason}", 
                    subscriptionId, reason);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error canceling subscription {SubscriptionId}", subscriptionId);
                throw;
            }
        }

        /// <summary>
        /// Get total active subscriptions count
        /// </summary>
        public async Task<int> GetActiveSubscriptionCountAsync()
        {
            try
            {
                var specs = new SubscriptionFilterSpecs(status: SubscriptionStatus.Active);
                return await _unitOfWork
                    .Repository<Subscription>()
                    .GetCountWithspecsAsync(specs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active subscription count");
                throw;
            }
        }

        /// <summary>
        /// Get expiring soon subscriptions (within 7 days)
        /// </summary>
        public async Task<IEnumerable<SubscriptionResponse>> GetExpiringSoonSubscriptionsAsync()
        {
            try
            {
                var allSubscriptions = await _unitOfWork
                    .Repository<Subscription>()
                    .GetAllAsync();

                var expiringSubscriptions = allSubscriptions
                    .Where(s => s.Status == SubscriptionStatus.Active &&
                                s.CurrentPeriodEnd >= DateTime.UtcNow &&
                                s.CurrentPeriodEnd <= DateTime.UtcNow.AddDays(7))
                    .ToList();

                return _mapper.Map<IEnumerable<SubscriptionResponse>>(expiringSubscriptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving expiring subscriptions");
                throw;
            }
        }
    }
}
