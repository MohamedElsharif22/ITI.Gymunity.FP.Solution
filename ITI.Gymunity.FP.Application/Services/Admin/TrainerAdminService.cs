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
    public class TrainerAdminService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<TrainerAdminService> logger)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<TrainerAdminService> _logger = logger;

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
            try
            {
                var trainer = await _unitOfWork
                    .Repository<TrainerProfile>()
                    .GetByIdAsync(id);

                if (trainer == null)
                    return null;

                return _mapper.Map<TrainerProfileDetailResponse>(trainer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving trainer with ID {TrainerId}", id);
                throw;
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
