using Adaptor.ExternalServices;
using Adaptor.Interfaces;
using Adaptor.Logging;
using Adaptor.PaymentProcessors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Adaptor.Tests
{
    /// <summary>
    /// Test class for the PayPalAdapter 
    /// </summary>
    [TestClass]
    public class PayPalAdapterTests
    {
        private readonly TestLogger _logger = new TestLogger();
        private readonly MockPayPalService _mockPayPalService = new MockPayPalService();
        
        /// <summary>
        /// Test class for logging that captures log messages
        /// </summary>
        private class TestLogger : ILogger
        {
            public string LastLogMessage { get; private set; } = string.Empty;
            
            public void LogInfo(string message) => LastLogMessage = message;
            public void LogWarning(string message) => LastLogMessage = message;
            public void LogError(string message) => LastLogMessage = message;
            public void LogSuccess(string message) => LastLogMessage = message;
        }
        
        /// <summary>
        /// Mock implementation of the PayPal service for testing
        /// </summary>
        private class MockPayPalService : PayPalService
        {
            public bool AuthorizeCalled { get; private set; }
            public bool CompleteCalled { get; private set; }
            public bool RefundCalled { get; private set; }
            public string LastEmail { get; private set; } = string.Empty;
            public double LastAmount { get; private set; }
            
            public override string AuthorizePayment(string emailAddress, double paymentAmount)
            {
                AuthorizeCalled = true;
                LastEmail = emailAddress;
                LastAmount = paymentAmount;
                return "TEST-TOKEN";
            }
            
            public override bool CompletePayment(string authorizationToken)
            {
                CompleteCalled = true;
                return true;
            }
            
            public override string RequestRefund(string emailAddress, string paymentId, double amount)
            {
                RefundCalled = true;
                LastEmail = emailAddress;
                LastAmount = amount;
                return "TEST-REFUND";
            }
        }
        
        /// <summary>
        /// Test that the adapter correctly processes a payment through PayPal
        /// </summary>
        [TestMethod]
        public void ProcessPayment_CallsPayPalService_ReturnsTrue()
        {
            // Arrange
            var adapter = new PayPalAdapter(_mockPayPalService, _logger);
            string customerId = "TEST001";
            decimal amount = 123.45m;
            
            // Act
            bool result = adapter.ProcessPayment(customerId, amount);
            
            // Assert
            Assert.IsTrue(result, "Payment processing should return true");
            Assert.IsTrue(_mockPayPalService.AuthorizeCalled, "AuthorizePayment should be called");
            Assert.IsTrue(_mockPayPalService.CompleteCalled, "CompletePayment should be called");
            Assert.AreEqual(123.45, _mockPayPalService.LastAmount, 0.001, "Amount should be converted correctly");
            Assert.IsTrue(_logger.LastLogMessage.Contains("successful"), "Success message should be logged");
        }
        
        /// <summary>
        /// Test that the adapter correctly processes a refund through PayPal
        /// </summary>
        [TestMethod]
        public void RefundPayment_WithValidTransaction_ReturnsTrue()
        {
            // Arrange
            var adapter = new PayPalAdapter(_mockPayPalService, _logger);
            
            // First process a payment to create a transaction
            adapter.ProcessPayment("TEST001", 100m);
            
            // Find the transaction ID (this is simplified for the test)
            string transactionId = "TXN"; // Partial match for the generated ID
            
            // Act & Assert - use reflection to get the private _transactionMap field
            var type = typeof(PayPalAdapter);
            var fieldInfo = type.GetField("_transactionMap", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (fieldInfo != null)
            {
                var transactionMap = fieldInfo.GetValue(adapter) as System.Collections.Generic.Dictionary<string, string>;
                if (transactionMap != null)
                {
                    string? actualTransactionId = null;
                    foreach (var key in transactionMap.Keys)
                    {
                        if (key.StartsWith(transactionId))
                        {
                            actualTransactionId = key;
                            break;
                        }
                    }
                    
                    if (actualTransactionId != null)
                    {
                        // Now refund the payment
                        bool result = adapter.RefundPayment(actualTransactionId, 50m);
                        
                        Assert.IsTrue(result, "Refund should return true");
                        Assert.IsTrue(_mockPayPalService.RefundCalled, "RequestRefund should be called");
                        Assert.AreEqual(50.0, _mockPayPalService.LastAmount, 0.001, "Refund amount should be converted correctly");
                    }
                    else
                    {
                        Assert.Fail("No transaction ID found");
                    }
                }
                else
                {
                    Assert.Fail("Could not access transaction map");
                }
            }
            else
            {
                Assert.Fail("Could not find _transactionMap field");
            }
        }
        
        /// <summary>
        /// Test that the adapter returns the correct payment status
        /// </summary>
        [TestMethod]
        public void GetPaymentStatus_WithValidTransaction_ReturnsStatus()
        {
            // Arrange
            var adapter = new PayPalAdapter(_mockPayPalService, _logger);
            
            // First process a payment to create a transaction
            adapter.ProcessPayment("TEST001", 100m);
            
            // Find the transaction ID (this is simplified for the test)
            string transactionId = "TXN"; // Partial match for the generated ID
            
            // Act & Assert - use reflection to get the private _transactionMap field
            var type = typeof(PayPalAdapter);
            var fieldInfo = type.GetField("_transactionMap", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (fieldInfo != null)
            {
                var transactionMap = fieldInfo.GetValue(adapter) as System.Collections.Generic.Dictionary<string, string>;
                if (transactionMap != null)
                {
                    string? actualTransactionId = null;
                    foreach (var key in transactionMap.Keys)
                    {
                        if (key.StartsWith(transactionId))
                        {
                            actualTransactionId = key;
                            break;
                        }
                    }
                    
                    if (actualTransactionId != null)
                    {
                        // Get the status
                        string result = adapter.GetPaymentStatus(actualTransactionId);
                        
                        Assert.IsTrue(result.Contains("status"), "Status should contain status information");
                        Assert.IsTrue(result.Contains(actualTransactionId), "Status should contain the transaction ID");
                    }
                    else
                    {
                        Assert.Fail("No transaction ID found");
                    }
                }
                else
                {
                    Assert.Fail("Could not access transaction map");
                }
            }
            else
            {
                Assert.Fail("Could not find _transactionMap field");
            }
        }
    }
}