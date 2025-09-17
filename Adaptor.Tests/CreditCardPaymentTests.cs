using Adaptor.Interfaces;
using Adaptor.Logging;
using Adaptor.PaymentProcessors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Adaptor.Tests
{
    /// <summary>
    /// Test class for the CreditCardPayment class
    /// </summary>
    [TestClass]
    public class CreditCardPaymentTests
    {
        private readonly TestLogger _logger = new TestLogger();
        
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
        /// Test that the credit card processor processes payments correctly
        /// </summary>
        [TestMethod]
        public void ProcessPayment_ReturnsTrue_AndLogsSuccess()
        {
            // Arrange
            var processor = new CreditCardPayment(_logger);
            string customerId = "CUST001";
            decimal amount = 123.45m;
            
            // Act
            bool result = processor.ProcessPayment(customerId, amount);
            
            // Assert
            Assert.IsTrue(result, "Payment processing should return true");
            Assert.IsTrue(_logger.LastLogMessage.Contains("successful"), "Success message should be logged");
        }
        
        /// <summary>
        /// Test that the credit card processor processes refunds correctly
        /// </summary>
        [TestMethod]
        public void RefundPayment_ReturnsTrue_AndLogsSuccess()
        {
            // Arrange
            var processor = new CreditCardPayment(_logger);
            string transactionId = "TX12345";
            decimal amount = 45.67m;
            
            // Act
            bool result = processor.RefundPayment(transactionId, amount);
            
            // Assert
            Assert.IsTrue(result, "Refund processing should return true");
            Assert.IsTrue(_logger.LastLogMessage.Contains("successful"), "Success message should be logged");
        }
        
        /// <summary>
        /// Test that the credit card processor returns the correct status
        /// </summary>
        [TestMethod]
        public void GetPaymentStatus_ReturnsFormattedStatus()
        {
            // Arrange
            var processor = new CreditCardPayment(_logger);
            string transactionId = "TX12345";
            
            // Act
            string status = processor.GetPaymentStatus(transactionId);
            
            // Assert
            Assert.IsTrue(status.Contains(transactionId), "Status should contain the transaction ID");
            Assert.IsTrue(status.Contains("status"), "Status should mention status");
        }
    }
}