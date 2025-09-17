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
        
        private class TestLogger : ILogger
        {
            public string LastLogMessage { get; private set; } = string.Empty;
            
            public void LogInfo(string message) => LastLogMessage = message;
            public void LogWarning(string message) => LastLogMessage = message;
            public void LogError(string message) => LastLogMessage = message;
            public void LogSuccess(string message) => LastLogMessage = message;
        }
        
  
        [TestMethod]
        public void ProcessPayment_ReturnsTrue_AndLogsSuccess()
        {
            var processor = new CreditCardPayment(_logger);
            string customerId = "CUST001";
            decimal amount = 123.45m;
            
            bool result = processor.ProcessPayment(customerId, amount);
            
            Assert.IsTrue(result, "Payment processing should return true");
            Assert.IsTrue(_logger.LastLogMessage.Contains("successful"), "Success message should be logged");
        }
        
        [TestMethod]
        public void RefundPayment_ReturnsTrue_AndLogsSuccess()
        {
            var processor = new CreditCardPayment(_logger);
            string transactionId = "TX12345";
            decimal amount = 45.67m;
            
            bool result = processor.RefundPayment(transactionId, amount);
            
            Assert.IsTrue(result, "Refund processing should return true");
            Assert.IsTrue(_logger.LastLogMessage.Contains("successful"), "Success message should be logged");
        }
        
        [TestMethod]
        public void GetPaymentStatus_ReturnsFormattedStatus()
        {
            var processor = new CreditCardPayment(_logger);
            string transactionId = "TX12345";
            
            string status = processor.GetPaymentStatus(transactionId);
            
            Assert.IsTrue(status.Contains(transactionId), "Status should contain the transaction ID");
            Assert.IsTrue(status.Contains("status"), "Status should mention status");
        }
    }
}