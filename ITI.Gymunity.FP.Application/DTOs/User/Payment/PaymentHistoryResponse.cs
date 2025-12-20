namespace ITI.Gymunity.FP.Infrastructure.DTOs.User.Payment
{
    public class PaymentHistoryResponse
    {
        public int TotalPayments { get; set; }
        public decimal TotalAmount { get; set; }
        public List<PaymentResponse> Payments { get; set; } = new();
    }
}