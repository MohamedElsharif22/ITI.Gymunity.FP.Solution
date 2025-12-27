using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.User.Payment;
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
    /// Admin service for managing payments, refunds, and payment analytics
    /// </summary>
    public class PaymentAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentAdminService> _logger;

        public PaymentAdminService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<PaymentAdminService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all payments with optional filtering and pagination
        /// </summary>
        public async Task<IEnumerable<PaymentResponse>> GetAllPaymentsAsync(
            PaymentFilterSpecs specs)
        {
            try
            {
                var payments = await _unitOfWork
                    .Repository<Domain.Models.Payment>()
                    .GetAllWithSpecsAsync(specs);

                return _mapper.Map<IEnumerable<PaymentResponse>>(payments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payments with specs");
                throw;
            }
        }

        /// <summary>
        /// Get payment by ID
        /// </summary>
        public async Task<PaymentResponse?> GetPaymentByIdAsync(int paymentId)
        {
            try
            {
                var payment = await _unitOfWork
                    .Repository<Domain.Models.Payment>()
                    .GetByIdAsync(paymentId);

                if (payment == null)
                    return null;

                return _mapper.Map<PaymentResponse>(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payment with ID {PaymentId}", paymentId);
                throw;
            }
        }

        /// <summary>
        /// Get count of payments matching specification
        /// </summary>
        public async Task<int> GetPaymentCountAsync(PaymentFilterSpecs specs)
        {
            try
            {
                return await _unitOfWork
                    .Repository<Domain.Models.Payment>()
                    .GetCountWithspecsAsync(specs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payment count");
                throw;
            }
        }

        /// <summary>
        /// Get failed payments
        /// </summary>
        public async Task<IEnumerable<PaymentResponse>> GetFailedPaymentsAsync(
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var specs = new PaymentFilterSpecs(
                    status: PaymentStatus.Failed,
                    pageNumber: pageNumber,
                    pageSize: pageSize);

                var payments = await _unitOfWork
                    .Repository<Domain.Models.Payment>()
                    .GetAllWithSpecsAsync(specs);

                return _mapper.Map<IEnumerable<PaymentResponse>>(payments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving failed payments");
                throw;
            }
        }

        /// <summary>
        /// Get completed payments
        /// </summary>
        public async Task<IEnumerable<PaymentResponse>> GetCompletedPaymentsAsync(
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var specs = new PaymentFilterSpecs(
                    status: PaymentStatus.Completed,
                    pageNumber: pageNumber,
                    pageSize: pageSize);

                var payments = await _unitOfWork
                    .Repository<Domain.Models.Payment>()
                    .GetAllWithSpecsAsync(specs);

                return _mapper.Map<IEnumerable<PaymentResponse>>(payments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving completed payments");
                throw;
            }
        }

        /// <summary>
        /// Get pending payments
        /// </summary>
        public async Task<IEnumerable<PaymentResponse>> GetPendingPaymentsAsync(
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var specs = new PaymentFilterSpecs(
                    status: PaymentStatus.Pending,
                    pageNumber: pageNumber,
                    pageSize: pageSize);

                var payments = await _unitOfWork
                    .Repository<Domain.Models.Payment>()
                    .GetAllWithSpecsAsync(specs);

                return _mapper.Map<IEnumerable<PaymentResponse>>(payments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pending payments");
                throw;
            }
        }

        /// <summary>
        /// Get refunded payments
        /// </summary>
        public async Task<IEnumerable<PaymentResponse>> GetRefundedPaymentsAsync(
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var specs = new PaymentFilterSpecs(
                    status: PaymentStatus.Refunded,
                    pageNumber: pageNumber,
                    pageSize: pageSize);

                var payments = await _unitOfWork
                    .Repository<Domain.Models.Payment>()
                    .GetAllWithSpecsAsync(specs);

                return _mapper.Map<IEnumerable<PaymentResponse>>(payments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving refunded payments");
                throw;
            }
        }

        /// <summary>
        /// Get revenue for a date range
        /// </summary>
        public async Task<decimal> GetRevenueAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var specs = new PaymentFilterSpecs(
                    status: PaymentStatus.Completed,
                    startDate: startDate,
                    endDate: endDate);

                var payments = await _unitOfWork
                    .Repository<Domain.Models.Payment>()
                    .GetAllWithSpecsAsync(specs);

                return payments.Sum(p => p.Amount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating revenue for date range");
                throw;
            }
        }

        /// <summary>
        /// Process refund for a payment
        /// </summary>
        public async Task<bool> ProcessRefundAsync(int paymentId)
        {
            try
            {
                var payment = await _unitOfWork
                    .Repository<Domain.Models.Payment>()
                    .GetByIdAsync(paymentId);

                if (payment == null)
                {
                    _logger.LogWarning("Payment with ID {PaymentId} not found for refund", paymentId);
                    return false;
                }

                if (payment.Status == PaymentStatus.Refunded)
                {
                    _logger.LogWarning("Payment {PaymentId} already refunded", paymentId);
                    return false;
                }

                payment.Status = PaymentStatus.Refunded;

                _unitOfWork.Repository<Domain.Models.Payment>().Update(payment);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("Payment {PaymentId} refunded successfully", paymentId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing refund for payment {PaymentId}", paymentId);
                throw;
            }
        }

        /// <summary>
        /// Get total revenue (all completed payments)
        /// </summary>
        public async Task<decimal> GetTotalRevenueAsync()
        {
            try
            {
                var specs = new PaymentFilterSpecs(status: PaymentStatus.Completed);
                var payments = await _unitOfWork
                    .Repository<Domain.Models.Payment>()
                    .GetAllWithSpecsAsync(specs);

                return payments.Sum(p => p.Amount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating total revenue");
                throw;
            }
        }

        /// <summary>
        /// Get count of failed payments
        /// </summary>
        public async Task<int> GetFailedPaymentCountAsync()
        {
            try
            {
                var specs = new PaymentFilterSpecs(status: PaymentStatus.Failed);
                return await _unitOfWork
                    .Repository<Domain.Models.Payment>()
                    .GetCountWithspecsAsync(specs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting failed payment count");
                throw;
            }
        }
    }
}
