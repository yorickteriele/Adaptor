namespace Adaptor.Interfaces
{
    /// <summary>
    /// Interface for payment processors - this is our Target in the Adapter pattern.
    /// All payment methods in our system must implement this interface.
    /// </summary>
    public interface IPaymentProcessor
    {
        /// <summary>
        /// Process a payment for the specified customer
        /// </summary>
        /// <param name="customerId">The unique identifier for the customer</param>
        /// <param name="amount">The payment amount</param>
        /// <returns>True if the payment was successful, false otherwise</returns>
        bool ProcessPayment(string customerId, decimal amount);

        /// <summary>
        /// Process a refund for a previous transaction
        /// </summary>
        /// <param name="transactionId">The unique identifier for the transaction to refund</param>
        /// <param name="amount">The amount to refund, which may be less than the original transaction</param>
        /// <returns>True if the refund was successful, false otherwise</returns>
        bool RefundPayment(string transactionId, decimal amount);

        /// <summary>
        /// Get the current status of a payment transaction
        /// </summary>
        /// <param name="transactionId">The unique identifier for the transaction</param>
        /// <returns>A string representation of the transaction status</returns>
        string GetPaymentStatus(string transactionId);
    }
}