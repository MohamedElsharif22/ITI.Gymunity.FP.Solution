using ITI.Gymunity.FP.Domain.Models;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Contracts.ExternalServices
{
    public interface IPayPalService
    {
        /// <summary>
        /// Creates a PayPal order for a subscription
        /// Uses Subscription context instead of Payment to align with new architecture
        /// </summary>
        Task<(bool Success, string? OrderId, string? ApprovalUrl, string? ErrorMessage)>
            CreateOrderAsync(Subscription subscription, string returnUrl, string cancelUrl);

        /// <summary>
        /// Captures a PayPal order (after user approves)
        /// </summary>
        Task<(bool Success, string? CaptureId, string? ErrorMessage)>
            CaptureOrderAsync(string orderId);

        /// <summary>
        /// Gets order details from PayPal
        /// </summary>
        Task<dynamic?> GetOrderAsync(string orderId);

        /// <summary>
        /// Refunds a PayPal capture
        /// </summary>
        Task<(bool Success, string? RefundId, string? ErrorMessage)>
            RefundCaptureAsync(string captureId, decimal amount, string currency);
    }
}
