using Adaptor.API.Interfaces;
using Adaptor.API.Models;

namespace Adaptor.API.Services
{
    public class CreditCardPayment : IPaymentProcessor
    {
        public async Task<PaymentResponse> ProcessPayment(decimal amount, PaymentRequest request)
        {
            await Task.Delay(500);
            
            if (string.IsNullOrEmpty(request.CardNumber))
            {
                return new PaymentResponse
                {
                    IsSuccess = false,
                    Message = "Card number required",
                    Amount = amount,
                    PaymentMethod = "CreditCard"
                };
            }
                
            return new PaymentResponse
            {
                IsSuccess = true,
                Message = $"Credit card payment successful for {request.CardHolderName}",
                Amount = amount,
                PaymentMethod = "CreditCard",
                TransactionId = $"CC_{Guid.NewGuid().ToString()[..8]}",
                ProcessedAt = DateTime.UtcNow
            };
        }
    }
}