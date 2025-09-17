namespace Adaptor.Interfaces
{
    /// <summary>
    /// Interface for payment processors - this is our Target in the Adapter pattern.
    /// All payment methods in our system must implement this interface.
    /// </summary>
    public interface IPaymentProcessor
    {
      
        bool ProcessPayment(string customerId, decimal amount);
      
        bool RefundPayment(string transactionId, decimal amount);

        string GetPaymentStatus(string transactionId);
    }
}