using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Specification;

namespace ITI.Gymunity.FP.Application.Specefications.Payment
{
    public class PaymentByGatewayOrderSpecs : BaseSpecification<Domain.Models.Payment>
    {
        // For Paymob
        public PaymentByGatewayOrderSpecs(string paymobOrderId)
            : base(p => p.PaymobOrderId == paymobOrderId && !p.IsDeleted)
        {
            AddInclude(p => p.Subscription);
        }

        // For PayPal
        public static PaymentByGatewayOrderSpecs ForPayPal(string paypalOrderId)
        {
            return new PaymentByGatewayOrderSpecs(
                p => p.PayPalOrderId == paypalOrderId && !p.IsDeleted);
        }

        private PaymentByGatewayOrderSpecs(
            System.Linq.Expressions.Expression<Func<Domain.Models.Payment, bool>> criteria)
            : base(criteria)
        {
            AddInclude(p => p.Subscription);
        }
    }
}