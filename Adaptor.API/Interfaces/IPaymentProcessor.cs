using Adaptor.API.Models;

namespace Adaptor.API.Interfaces
{
    public interface IPaymentProcessor
    {
        Task<PaymentResponse> ProcessPayment(decimal amount, PaymentRequest request);
    }
}