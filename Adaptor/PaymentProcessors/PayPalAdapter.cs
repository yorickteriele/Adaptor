using System;
using System.Collections.Generic;
using Adaptor.ExternalServices;
using Adaptor.Interfaces;
using Adaptor.Logging;

namespace Adaptor.PaymentProcessors
{
    /// <summary>
    /// Adapter for PayPal service that implements the IPaymentProcessor interface.
    /// This is our Adapter in the Adapter pattern.
    /// </summary>
    public class PayPalAdapter : IPaymentProcessor
    {
        private readonly PayPalService _payPalService;
        private readonly ILogger _logger;
        private readonly Dictionary<string, string> _transactionMap = new();
        private readonly Dictionary<string, string> _customerEmails = new();

        /// <summary>
        /// Constructor for the PayPalAdapter
        /// </summary>
        /// <param name="payPalService">PayPal service instance (optional)</param>
        /// <param name="logger">Logger instance (optional)</param>
        public PayPalAdapter(PayPalService? payPalService = null, ILogger? logger = null)
        {
            _payPalService = payPalService ?? new PayPalService();
            _logger = logger ?? new ConsoleLogger();
            
            // For demo purposes, we'll store some fake customer email mappings
            _customerEmails.Add("CUST001", "klant1@example.nl");
            _customerEmails.Add("CUST002", "klant2@example.nl");
            _customerEmails.Add("CUST003", "klant3@example.nl");
        }

        /// <summary>
        /// Process a payment through PayPal
        /// </summary>
        /// <param name="customerId">The unique identifier for the customer</param>
        /// <param name="amount">The payment amount</param>
        /// <returns>True if the payment was successful, false otherwise</returns>
        public bool ProcessPayment(string customerId, decimal amount)
        {
            try
            {
                // Convert our system's data to what PayPal API expects
                string customerEmail = GetCustomerEmail(customerId);
                double paypalAmount = Convert.ToDouble(amount);
                
                _logger.LogInfo($"PayPal Adapter: Processing payment for customer {customerId}");
                _logger.LogInfo($"Email: {customerEmail}, Amount: €{amount:F2}");
                
                // Call the PayPal API using its own interface
                string authToken = _payPalService.AuthorizePayment(customerEmail, paypalAmount);
                bool success = _payPalService.CompletePayment(authToken);
                
                // Store the mapping between our system's transaction IDs and PayPal's
                if (success)
                {
                    string transactionId = "TXN" + Guid.NewGuid().ToString().Substring(0, 8);
                    _transactionMap[transactionId] = authToken;
                    _logger.LogSuccess($"PayPal payment successful! Transaction ID: {transactionId}");
                }
                else
                {
                    _logger.LogError("PayPal payment failed!");
                }
                
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError($"PayPal Adapter Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Process a refund through PayPal
        /// </summary>
        /// <param name="transactionId">The unique identifier for the transaction to refund</param>
        /// <param name="amount">The amount to refund</param>
        /// <returns>True if the refund was successful, false otherwise</returns>
        public bool RefundPayment(string transactionId, decimal amount)
        {
            try
            {
                // Check if we have a mapping for this transaction ID
                if (!_transactionMap.ContainsKey(transactionId))
                {
                    _logger.LogError($"PayPal Adapter Error: Unknown transaction ID {transactionId}");
                    return false;
                }

                // Get the PayPal authorization token
                string paypalToken = _transactionMap[transactionId];
                
                _logger.LogInfo($"PayPal Adapter: Processing refund for transaction {transactionId}");
                _logger.LogInfo($"Amount: €{amount:F2}");
                
                // Determine customer email from the transaction (simplified for demo)
                string customerEmail = "unknown@example.nl";
                foreach (var pair in _customerEmails)
                {
                    customerEmail = pair.Value;  // Just use the first one for demo purposes
                    break;
                }
                
                // Call PayPal's refund API with its own interface
                string refundId = _payPalService.RequestRefund(customerEmail, paypalToken, Convert.ToDouble(amount));
                _logger.LogSuccess($"PayPal refund successful! Refund ID: {refundId}");
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"PayPal Adapter Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Get the current status of a PayPal transaction
        /// </summary>
        /// <param name="transactionId">The unique identifier for the transaction</param>
        /// <returns>A string representation of the transaction status</returns>
        public string GetPaymentStatus(string transactionId)
        {
            try
            {
                // Check if we have a mapping for this transaction ID
                if (!_transactionMap.ContainsKey(transactionId))
                {
                    return $"Transaction {transactionId} not found in PayPal records";
                }

                // Get the PayPal authorization token
                string paypalToken = _transactionMap[transactionId];
                
                // Call PayPal API to get status
                var statusData = _payPalService.CheckTransactionStatus(paypalToken);
                
                // Convert PayPal's response format to our system's format
                return $"PayPal transaction {transactionId} status: {statusData["status"]}";
            }
            catch (Exception ex)
            {
                _logger.LogError($"PayPal Adapter Error getting status: {ex.Message}");
                return $"Error retrieving status for transaction {transactionId}";
            }
        }
        
        /// <summary>
        /// Helper method to get customer email from ID
        /// </summary>
        /// <param name="customerId">The unique identifier for the customer</param>
        /// <returns>The customer's email address</returns>
        private string GetCustomerEmail(string customerId)
        {
            if (_customerEmails.ContainsKey(customerId))
            {
                return _customerEmails[customerId];
            }
            
            // For demo purposes, return a fake email
            return $"{customerId}@example.nl";
        }
    }
}