using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Application.Specefications.Admin;
using ITI.Gymunity.FP.Domain;
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
            var trainer = await repo.GetWithSpecsAsync(new ITI.Gymunity.FP.Application.Specefications.Trainer.TrainerByUserIdSpecs("") );
            // Attempt to get by id using repository GetByIdAsync
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

        private async Task<decimal> ComputeAvailableBalanceAsync(int trainerProfileId)
        {
            // Find package ids for trainer
            var packageRepo = _unitOfWork.Repository<ITI.Gymunity.FP.Domain.Models.Trainer.Package>();
            var packages = await packageRepo.GetAllAsync();
            var trainerPackageIds = packages.Where(p => p.TrainerId == trainerProfileId).Select(p => p.Id).ToList();

            if (!trainerPackageIds.Any()) return 0m;

            // Query payments with specs: payments where subscription.packageId in trainerPackageIds and status == Completed
            var paymentRepo = _unitOfWork.Repository<ITI.Gymunity.FP.Domain.Models.Payment>();
            var allPayments = await paymentRepo.GetAllAsync();

            var paymentsForTrainer = allPayments.Where(p => p.Status == ITI.Gymunity.FP.Domain.Models.Enums.PaymentStatus.Completed
                                                           && trainerPackageIds.Contains(p.Subscription.PackageId));

            var totalPayout = paymentsForTrainer.Sum(p => p.TrainerPayout);

            // If there is a Payouts table or records of payouts to trainers, subtract them here.
            // Currently there's no payouts entity in the project; assume platform holds funds until admin pays out.

            return totalPayout;
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
