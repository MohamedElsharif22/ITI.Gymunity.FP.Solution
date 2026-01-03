using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Application.Specefications.Admin;
using ITI.Gymunity.FP.Domain;
using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Services.Admin
{
    /// <summary>
    /// Admin service for managing trainer profiles, verification, and actions
    /// </summary>
    public class TrainerAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<TrainerAdminService> _logger;

        public TrainerAdminService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TrainerAdminService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all trainers with optional filtering and pagination
        /// </summary>
        public async Task<IEnumerable<TrainerProfileDetailResponse>> GetAllTrainersAsync(
            TrainerFilterSpecs specs)
        {
            try
            {
                var trainers = await _unitOfWork
                    .Repository<TrainerProfile>()
                    .GetAllWithSpecsAsync(specs);

                return _mapper.Map<IEnumerable<TrainerProfileDetailResponse>>(trainers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving trainers with specs");
                throw;
            }
        }

        /// <summary>
        /// Get trainer by ID with full details
        /// </summary>
        public async Task<TrainerProfileDetailResponse?> GetTrainerByIdAsync(int id)
        {
            var repo = _unitOfWork.Repository<TrainerProfile>();
            var entity = await repo.GetByIdAsync(id);
            if (entity == null) return null;

            var dto = _mapper.Map<TrainerProfileDetailResponse>(entity);

            // Compute available balance: sum of TrainerPayout of completed payments for subscriptions that belong to trainer's packages
            try
            {
                decimal available = await ComputeAvailableBalanceAsync(entity.Id);
                dto.AvailableBalance = available;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error computing available balance for trainer {TrainerId}", id);
                dto.AvailableBalance = 0m;
            }

            return dto;
        }

        /// <summary>
        /// Get trainer details for admin view including earnings and reviews
        /// </summary>
        public async Task<(TrainerProfileDetailResponse? trainer, List<TrainerReview> reviews, decimal totalEarnings, decimal platformFees, int completedPaymentsCount)> GetTrainerDetailsForAdminAsync(int id)
        {
            try
            {
                // Use specification to load trainer with user data (email, username)
                var detailSpec = new TrainerDetailSpecs(id);
                var trainers = await _unitOfWork.Repository<TrainerProfile>()
                    .GetAllWithSpecsAsync(detailSpec);
                
                var entity = trainers.FirstOrDefault();
                if (entity == null) return (null, new(), 0m, 0m, 0);

                var dto = _mapper.Map<TrainerProfileDetailResponse>(entity);

                // Get available balance
                try
                {
                    decimal available = await ComputeAvailableBalanceAsync(entity.Id);
                    dto.AvailableBalance = available;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error computing available balance for trainer {TrainerId}", id);
                    dto.AvailableBalance = 0m;
                }

                // Get trainer reviews using specification
                var reviewsSpec = new TrainerReviewsSpecs(id, pageNumber: 1, pageSize: 10);
                var reviews = await _unitOfWork.Repository<ITI.Gymunity.FP.Domain.Models.Trainer.TrainerReview>()
                    .GetAllWithSpecsAsync(reviewsSpec);

                // Get trainer earnings and platform fees using specification
                var paymentsSpec = new TrainerPaymentsEarningsSpecs(id, allRecords: true);
                var payments = await _unitOfWork.Repository<ITI.Gymunity.FP.Domain.Models.Payment>()
                    .GetAllWithSpecsAsync(paymentsSpec);

                // Calculate totals from payments
                var totalEarnings = payments.Sum(p => p.TrainerPayout);
                var platformFees = payments.Sum(p => p.Amount - p.TrainerPayout);
                var completedPaymentsCount = payments.Count();

                return (dto, reviews.ToList(), totalEarnings, platformFees, completedPaymentsCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting trainer details for admin view: {TrainerId}", id);
                throw;
            }
        }

        private async Task<decimal> ComputeAvailableBalanceAsync(int trainerProfileId)
        {
            try
            {
                // Use specification pattern to load trainer's packages with completed payments
                var spec = new TrainerPackagesWithEarningsSpecs(trainerProfileId);
                var packages = await _unitOfWork.Repository<Package>()
                    .GetAllWithSpecsAsync(spec);

                if (!packages.Any()) 
                {
                    _logger.LogDebug("No packages found for trainer {TrainerId}", trainerProfileId);
                    return 0m;
                }

                // Calculate total trainer payout from all completed payments across all packages
                var totalPayout = packages
                    .SelectMany(p => p.Subscriptions ?? new List<Subscription>())  // Get all subscriptions for all packages
                    .SelectMany(s => s.Payments ?? new List<Payment>())             // Get all payments for each subscription
                    .Where(p => p.Status == PaymentStatus.Completed && !p.IsDeleted) // Only completed, non-deleted payments
                    .Sum(p => p.TrainerPayout);                                     // Sum trainer payouts

                _logger.LogDebug("Calculated available balance for trainer {TrainerId}: {Balance}", trainerProfileId, totalPayout);
                return totalPayout;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error computing available balance for trainer {TrainerId}", trainerProfileId);
                // Return 0 on error to prevent service failure
                return 0m;
            }
        }

        /// <summary>
        /// Get count of trainers matching specification
        /// </summary>
        public async Task<int> GetTrainerCountAsync(TrainerFilterSpecs specs)
        {
            try
            {
                return await _unitOfWork
                    .Repository<TrainerProfile>()
                    .GetCountWithspecsAsync(specs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting trainer count");
                throw;
            }
        }

        /// <summary>
        /// Verify a trainer profile (mark as verified)
        /// </summary>
        public async Task<bool> VerifyTrainerAsync(int trainerId)
        {
            try
            {
                var trainer = await _unitOfWork
                    .Repository<TrainerProfile>()
                    .GetByIdAsync(trainerId);

                if (trainer == null)
                {
                    _logger.LogWarning("Trainer with ID {TrainerId} not found for verification", trainerId);
                    return false;
                }

                trainer.IsVerified = true;
                trainer.VerifiedAt = DateTime.UtcNow;

                _unitOfWork.Repository<TrainerProfile>().Update(trainer);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("Trainer {TrainerId} verified successfully", trainerId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying trainer {TrainerId}", trainerId);
                throw;
            }
        }

        /// <summary>
        /// Reject trainer verification (soft delete)
        /// </summary>
        public async Task<bool> RejectTrainerAsync(int trainerId)
        {
            try
            {
                var trainer = await _unitOfWork
                    .Repository<TrainerProfile>()
                    .GetByIdAsync(trainerId);

                if (trainer == null)
                {
                    _logger.LogWarning("Trainer with ID {TrainerId} not found for rejection", trainerId);
                    return false;
                }

                trainer.IsVerified = false;
                trainer.IsDeleted = true;

                _unitOfWork.Repository<TrainerProfile>().Update(trainer);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("Trainer {TrainerId} rejected and soft-deleted", trainerId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting trainer {TrainerId}", trainerId);
                throw;
            }
        }

        /// <summary>
        /// Suspend/Reactivate trainer account
        /// </summary>
        public async Task<bool> SuspendTrainerAsync(int trainerId, bool suspend = true)
        {
            try
            {
                var trainer = await _unitOfWork
                    .Repository<TrainerProfile>()
                    .GetByIdAsync(trainerId);

                if (trainer == null)
                {
                    _logger.LogWarning("Trainer with ID {TrainerId} not found for suspension", trainerId);
                    return false;
                }

                trainer.IsSuspended = suspend;
                if (suspend)
                {
                    trainer.SuspendedAt = DateTime.UtcNow;
                }
                else
                {
                    trainer.SuspendedAt = null;
                    trainer.IsDeleted = false;
                }

                _unitOfWork.Repository<TrainerProfile>().Update(trainer);
                await _unitOfWork.CompleteAsync();

                var action = suspend ? "suspended" : "reactivated";
                _logger.LogInformation("Trainer {TrainerId} {Action}", trainerId, action);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error suspending trainer {TrainerId}", trainerId);
                throw;
            }
        }

        /// <summary>
        /// Get pending trainer profiles awaiting verification
        /// </summary>
        public async Task<IEnumerable<TrainerProfileDetailResponse>> GetPendingTrainersAsync(
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var specs = new TrainerFilterSpecs(
                    isVerified: false,
                    isSuspended: false,
                    pageNumber: pageNumber,
                    pageSize: pageSize);

                var trainers = await _unitOfWork
                    .Repository<TrainerProfile>()
                    .GetAllWithSpecsAsync(specs);

                return _mapper.Map<IEnumerable<TrainerProfileDetailResponse>>(trainers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pending trainers");
                throw;
            }
        }

        /// <summary>
        /// Get count of pending trainers
        /// </summary>
        public async Task<int> GetPendingTrainerCountAsync()
        {
            try
            {
                var specs = new TrainerFilterSpecs(isVerified: false, isSuspended: false);
                return await _unitOfWork
                    .Repository<TrainerProfile>()
                    .GetCountWithspecsAsync(specs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending trainer count");
                throw;
            }
        }

        /// <summary>
        /// Get verified trainers
        /// </summary>
        public async Task<IEnumerable<TrainerProfileDetailResponse>> GetVerifiedTrainersAsync(
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var specs = new TrainerFilterSpecs(
                    isVerified: true,
                    isSuspended: false,
                    pageNumber: pageNumber,
                    pageSize: pageSize);

                var trainers = await _unitOfWork
                    .Repository<TrainerProfile>()
                    .GetAllWithSpecsAsync(specs);

                return _mapper.Map<IEnumerable<TrainerProfileDetailResponse>>(trainers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving verified trainers");
                throw;
            }
        }

        /// <summary>
        /// Get suspended trainers
        /// </summary>
        public async Task<IEnumerable<TrainerProfileDetailResponse>> GetSuspendedTrainersAsync(
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var specs = new TrainerFilterSpecs(
                    isSuspended: true,
                    pageNumber: pageNumber,
                    pageSize: pageSize);

                var trainers = await _unitOfWork
                    .Repository<TrainerProfile>()
                    .GetAllWithSpecsAsync(specs);

                return _mapper.Map<IEnumerable<TrainerProfileDetailResponse>>(trainers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving suspended trainers");
                throw;
            }
        }

        /// <summary>
        /// Get count of suspended trainers
        /// </summary>
        public async Task<int> GetSuspendedTrainerCountAsync()
        {
            try
            {
                var specs = new TrainerFilterSpecs(isSuspended: true);
                return await _unitOfWork
                    .Repository<TrainerProfile>()
                    .GetCountWithspecsAsync(specs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting suspended trainer count");
                throw;
            }
        }

        /// <summary>
        /// Search trainers by name, email, or handle
        /// </summary>
        public async Task<IEnumerable<TrainerProfileDetailResponse>> SearchTrainersAsync(
            string searchTerm,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var specs = new TrainerFilterSpecs(
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    searchTerm: searchTerm);

                var trainers = await _unitOfWork
                    .Repository<TrainerProfile>()
                    .GetAllWithSpecsAsync(specs);

                return _mapper.Map<IEnumerable<TrainerProfileDetailResponse>>(trainers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching trainers with term {SearchTerm}", searchTerm);
                throw;
            }
        }
    }
}
