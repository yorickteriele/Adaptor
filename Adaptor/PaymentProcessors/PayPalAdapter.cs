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

        public PayPalAdapter(PayPalService? payPalService = null, ILogger? logger = null)
        {
            _payPalService = payPalService ?? new PayPalService();
            _logger = logger ?? new ConsoleLogger();
            
            // For demo purposes, we'll store some fake customer email mappings
            _customerEmails.Add("CUST001", "klant1@example.nl");
            _customerEmails.Add("CUST002", "klant2@example.nl");
            _customerEmails.Add("CUST003", "klant3@example.nl");
        }

        public bool ProcessPayment(string customerId, decimal amount)
        {
            try
            {
                string customerEmail = GetCustomerEmail(customerId);
                double paypalAmount = Convert.ToDouble(amount);
                
                _logger.LogInfo($"PayPal Adapter: Processing payment for customer {customerId}");
                _logger.LogInfo($"Email: {customerEmail}, Amount: €{amount:F2}");
                
                string authToken = _payPalService.AuthorizePayment(customerEmail, paypalAmount);
                bool success = _payPalService.CompletePayment(authToken);
                
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

        public bool RefundPayment(string transactionId, decimal amount)
        {
            try
            {
                if (!_transactionMap.ContainsKey(transactionId))
                {
                    _logger.LogError($"PayPal Adapter Error: Unknown transaction ID {transactionId}");
                    return false;
                }

                string paypalToken = _transactionMap[transactionId];
                
                _logger.LogInfo($"PayPal Adapter: Processing refund for transaction {transactionId}");
                _logger.LogInfo($"Amount: €{amount:F2}");
                
                string customerEmail = "unknown@example.nl";
                foreach (var pair in _customerEmails)
                {
                    customerEmail = pair.Value;  
                    break;
                }
                
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

        public string GetPaymentStatus(string transactionId)
        {
            try
            {
                if (!_transactionMap.ContainsKey(transactionId))
                {
                    return $"Transaction {transactionId} not found in PayPal records";
                }

                string paypalToken = _transactionMap[transactionId];
                
                var statusData = _payPalService.CheckTransactionStatus(paypalToken);
                
                return $"PayPal transaction {transactionId} status: {statusData["status"]}";
            }
            catch (Exception ex)
            {
                _logger.LogError($"PayPal Adapter Error getting status: {ex.Message}");
                return $"Error retrieving status for transaction {transactionId}";
            }
        }
        
        private string GetCustomerEmail(string customerId)
        {
            if (_customerEmails.ContainsKey(customerId))
            {
                return _customerEmails[customerId];
            }
            
            return $"{customerId}@example.nl";
        }
    }
}