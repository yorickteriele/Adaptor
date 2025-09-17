using Adaptor.Interfaces;
using Adaptor.Logging;
using Adaptor.Models;
using System;
using System.Collections.Generic;

namespace Adaptor.PaymentProcessors
{
    /// <summary>
    /// Concrete implementation of IPaymentProcessor for credit card payments.
    /// This class was already part of the system.
    /// </summary>
    public class CreditCardPayment : IPaymentProcessor
    {
        private readonly ILogger _logger;
        private readonly Dictionary<string, TransactionRecord> _transactions;

        public CreditCardPayment(ILogger? logger = null)
        {
            _logger = logger ?? new ConsoleLogger();
            _transactions = new Dictionary<string, TransactionRecord>();
        }

        /// <summary>
        /// Process a credit card payment for the specified customer
        /// </summary>
        /// <param name="customerId">The unique identifier for the customer</param>
        /// <param name="amount">The payment amount</param>
        /// <returns>True if the payment was successful, false otherwise</returns>
        public bool ProcessPayment(string customerId, decimal amount)
        {
            try
            {
                // In a real implementation, this would contact a payment gateway
                _logger.LogInfo($"Processing credit card payment for customer {customerId}");
                _logger.LogInfo($"Amount: €{amount:F2}");
                
                // Generate a transaction ID and store transaction data
                string transactionId = "CC" + Guid.NewGuid().ToString().Substring(0, 8);
                _transactions[transactionId] = new TransactionRecord
                {
                    CustomerId = customerId,
                    Amount = amount,
                    Timestamp = DateTime.Now,
                    Status = "COMPLETED"
                };
                
                _logger.LogSuccess("Credit card payment successful!");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Credit card payment failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Process a refund for a previous credit card transaction
        /// </summary>
        /// <param name="transactionId">The unique identifier for the transaction to refund</param>
        /// <param name="amount">The amount to refund</param>
        /// <returns>True if the refund was successful, false otherwise</returns>
        public bool RefundPayment(string transactionId, decimal amount)
        {
            try
            {
                // In a real implementation, this would contact a payment gateway
                _logger.LogWarning($"Refunding credit card payment for transaction {transactionId}");
                _logger.LogWarning($"Amount: €{amount:F2}");
                
                // Check if the transaction exists
                if (_transactions.ContainsKey(transactionId))
                {
                    var transaction = _transactions[transactionId];
                    transaction.Status = "REFUNDED";
                    transaction.RefundAmount = amount;
                    transaction.RefundTimestamp = DateTime.Now;
                }
                
                _logger.LogSuccess("Credit card refund successful!");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Credit card refund failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Get the current status of a credit card transaction
        /// </summary>
        /// <param name="transactionId">The unique identifier for the transaction</param>
        /// <returns>A string representation of the transaction status</returns>
        public string GetPaymentStatus(string transactionId)
        {
            if (_transactions.ContainsKey(transactionId))
            {
                return $"Credit card transaction {transactionId} status: {_transactions[transactionId].Status}";
            }
            
            return $"Credit card transaction {transactionId} status: Unknown";
        }
    }
}