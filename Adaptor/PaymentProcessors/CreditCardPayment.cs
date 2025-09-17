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

        public bool ProcessPayment(string customerId, decimal amount)
        {
            try
            {
                _logger.LogInfo($"Processing credit card payment for customer {customerId}");
                _logger.LogInfo($"Amount: €{amount:F2}");
                
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

        public bool RefundPayment(string transactionId, decimal amount)
        {
            try
            {
                _logger.LogWarning($"Refunding credit card payment for transaction {transactionId}");
                _logger.LogWarning($"Amount: €{amount:F2}");
                
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