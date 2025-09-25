    using Adaptor.API.Interfaces;
using Adaptor.API.Models;

namespace Adaptor.API.Services
{
    public class PayPalAdapter : IPaymentProcessor
    {
        private readonly PayPalService _payPalService;

        public PayPalAdapter(PayPalService payPalService)
        {
            _payPalService = payPalService;
        }

        public async Task<PaymentResponse> ProcessPayment(decimal amount, PaymentRequest request)
        {
            if (string.IsNullOrEmpty(request.PayPalEmail))
            {
                return new PaymentResponse
                {
                    IsSuccess = false,
                    Message = "PayPal email required",
                    Amount = amount,
                    PaymentMethod = "PayPal"
                };
            }

            var paypalResult = await _payPalService.SendMoney(request.PayPalEmail, amount);

            return new PaymentResponse
            {
                IsSuccess = paypalResult.Success,
                Message = paypalResult.Success 
                    ? $"PayPal payment successful. Fee: €{paypalResult.Fee:F2}" 
                    : paypalResult.Error,
                Amount = amount,
                PaymentMethod = "PayPal",
                TransactionId = paypalResult.PayPalTransactionId,
                ProcessedAt = DateTime.UtcNow
            };
        }
    }
}