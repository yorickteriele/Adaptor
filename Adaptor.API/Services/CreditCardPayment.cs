using Adaptor.API.Interfaces;
using Adaptor.API.Models;

namespace Adaptor.API.Services
{
    public class CreditCardPayment : IPaymentProcessor
    {
        private readonly PayPalService _payPalService;

        public async Task<PaymentResponse> ProcessPayment(decimal amount, PaymentRequest request)
        {
            var responses = new PaymentResponse();

            await Task.Delay(500);
            
            if (string.IsNullOrEmpty(request.CardNumber))
            {
                responses = (new PaymentResponse
                {
                    IsSuccess = false,
                    Message = "Card number required",
                    Amount = amount,
                    PaymentMethod = "CreditCard"
                });
            } 

            if(!string.IsNullOrEmpty(request.CardNumber)){
                responses = new PaymentResponse {
                    IsSuccess = true,
                    Message = $"Credit card payment successful for {request.CardHolderName}",
                    Amount = amount,
                    PaymentMethod = "CreditCard",
                    TransactionId = $"CC_{Guid.NewGuid().ToString()[..8]}",
                    ProcessedAt = DateTime.UtcNow
                };
            }

            if (string.IsNullOrEmpty(request.PayPalEmail)) {
                responses = new PaymentResponse {
                    IsSuccess = false,
                    Message = "PayPal email required",
                    Amount = amount,
                    PaymentMethod = "PayPal"
                };
            } 

            if(string.IsNullOrEmpty(request.PayPalEmail)){
                var paypalResult = await _payPalService.SendMoney(request.PayPalEmail, amount);

                responses = new PaymentResponse{
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

            return responses;
        }
    }
}