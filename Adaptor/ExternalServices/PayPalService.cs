using System;
using System.Collections.Generic;

namespace Adaptor.ExternalServices
{
    /// <summary>
    /// This class represents the external PayPal API - this is our Adaptee in the Adapter pattern.
    /// Notice how its interface is completely different from our IPaymentProcessor.
    /// </summary>
    public class PayPalService
    {
        private readonly Dictionary<string, PayPalTransactionInfo> _transactions = new();

        /// <summary>
        /// Authorize a payment with the PayPal API
        /// </summary>
        /// <param name="emailAddress">The PayPal email address of the payer</param>
        /// <param name="paymentAmount">The amount to be paid</param>
        /// <returns>An authorization token for the payment</returns>
        public virtual string AuthorizePayment(string emailAddress, double paymentAmount)
        {
            if (string.IsNullOrEmpty(emailAddress))
            {
                throw new ArgumentException("Email address is required", nameof(emailAddress));
            }

            if (paymentAmount <= 0)
            {
                throw new ArgumentException("Payment amount must be positive", nameof(paymentAmount));
            }

            // Simulate PayPal API call
            Console.WriteLine($"PayPal API: Authorizing payment for {emailAddress}");
            
            var authToken = Guid.NewGuid().ToString();
            
            // Store transaction info
            _transactions[authToken] = new PayPalTransactionInfo
            {
                Email = emailAddress,
                Amount = paymentAmount,
                Status = "PENDING",
                Created = DateTime.Now
            };
            
            return authToken;
        }

        /// <summary>
        /// Complete a previously authorized payment
        /// </summary>
        /// <param name="authorizationToken">The authorization token from AuthorizePayment</param>
        /// <returns>True if the payment was completed successfully, false otherwise</returns>
        public virtual bool CompletePayment(string authorizationToken)
        {
            if (string.IsNullOrEmpty(authorizationToken))
            {
                throw new ArgumentException("Authorization token is required", nameof(authorizationToken));
            }

            if (!_transactions.ContainsKey(authorizationToken))
            {
                throw new ArgumentException("Invalid authorization token", nameof(authorizationToken));
            }

            Console.WriteLine($"PayPal API: Completing payment with token {authorizationToken}");
            
            // Update transaction status
            var transaction = _transactions[authorizationToken];
            transaction.Status = "COMPLETED";
            transaction.Completed = DateTime.Now;
            
            return true;
        }

        /// <summary>
        /// Request a refund for a previous payment
        /// </summary>
        /// <param name="emailAddress">The PayPal email address of the payer</param>
        /// <param name="paymentId">The payment ID or authorization token</param>
        /// <param name="amount">The amount to refund</param>
        /// <returns>A refund ID</returns>
        public virtual string RequestRefund(string emailAddress, string paymentId, double amount)
        {
            if (string.IsNullOrEmpty(emailAddress))
            {
                throw new ArgumentException("Email address is required", nameof(emailAddress));
            }

            if (string.IsNullOrEmpty(paymentId))
            {
                throw new ArgumentException("Payment ID is required", nameof(paymentId));
            }

            if (amount <= 0)
            {
                throw new ArgumentException("Refund amount must be positive", nameof(amount));
            }

            if (!_transactions.ContainsKey(paymentId))
            {
                throw new ArgumentException("Invalid payment ID", nameof(paymentId));
            }

            Console.WriteLine($"PayPal API: Requesting refund to {emailAddress} for payment {paymentId}");
            
            // Update transaction status
            var transaction = _transactions[paymentId];
            transaction.Status = "REFUNDED";
            transaction.Refunded = DateTime.Now;
            transaction.RefundAmount = amount;
            
            // Generate a refund ID
            return "REF" + Guid.NewGuid().ToString().Substring(0, 8);
        }

        /// <summary>
        /// Check the status of a transaction
        /// </summary>
        /// <param name="paymentId">The payment ID or authorization token</param>
        /// <returns>A dictionary with transaction details</returns>
        public virtual Dictionary<string, string> CheckTransactionStatus(string paymentId)
        {
            if (string.IsNullOrEmpty(paymentId))
            {
                throw new ArgumentException("Payment ID is required", nameof(paymentId));
            }

            if (!_transactions.ContainsKey(paymentId))
            {
                return new Dictionary<string, string>
                {
                    {"id", paymentId},
                    {"status", "NOT_FOUND"},
                    {"date", DateTime.Now.ToString()}
                };
            }

            var transaction = _transactions[paymentId];
            
            return new Dictionary<string, string>
            {
                {"id", paymentId},
                {"email", transaction.Email},
                {"amount", transaction.Amount.ToString()},
                {"status", transaction.Status},
                {"created", transaction.Created.ToString()},
                {"completed", transaction.Completed?.ToString() ?? "N/A"},
                {"refunded", transaction.Refunded?.ToString() ?? "N/A"},
                {"refund_amount", transaction.RefundAmount?.ToString() ?? "N/A"}
            };
        }
    }

    /// <summary>
    /// Class to store PayPal transaction information
    /// </summary>
    internal class PayPalTransactionInfo
    {
        public string Email { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public DateTime? Completed { get; set; }
        public DateTime? Refunded { get; set; }
        public double? RefundAmount { get; set; }
    }
}