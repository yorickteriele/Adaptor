using System;
using Adaptor.Interfaces;
using Adaptor.PaymentProcessors;
using Adaptor.ExternalServices;
using Adaptor.PaymentProcessors;

namespace Adaptor.Tests
{
    [TestFixture]
    public class CreditCardPaymentTests
    {
        private CreditCardPayment _creditCardPayment;
        
<<<<<<< HEAD
        [SetUp]
        public void Setup()
=======
        private class TestLogger : ILogger
>>>>>>> origin/master
        {
            _creditCardPayment = new CreditCardPayment();
        }
        
<<<<<<< HEAD
        [Test]
        public void ProcessPayment_StandardAmount_ReturnsTrue()
        {
            var result = _creditCardPayment.ProcessPayment("CUST123", 50.00m);
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void RefundPayment_ValidTransaction_ReturnsTrue()
        {
            var result = _creditCardPayment.RefundPayment("TX123456", 25.50m);
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void GetPaymentStatus_ReturnsExpectedFormat()
        {
            var status = _creditCardPayment.GetPaymentStatus("TX9876");
            
            Assert.That(status, Does.Contain("status"));
            Assert.That(status, Does.Contain("TX9876"));
        }
        
        [Test]
        public void ProcessPayment_ZeroAmount_ReturnsTrue()
        {
            var result = _creditCardPayment.ProcessPayment("CUST999", 0m);
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void ProcessAndGetStatus_ReturnsNonEmptyStatus()
        {
            _creditCardPayment.ProcessPayment("CUST456", 50.00m);
            var status = _creditCardPayment.GetPaymentStatus("TX12345");
            Assert.That(status, Is.Not.Empty);
        }
        
        [Test]
        public void ProcessPayment_VariousAmounts_AllSucceed()
        {
            Assert.Multiple(() => {
                Assert.That(_creditCardPayment.ProcessPayment("CUST1", 10.00m), Is.True);
                Assert.That(_creditCardPayment.ProcessPayment("CUST2", 99.99m), Is.True);
                Assert.That(_creditCardPayment.ProcessPayment("CUST3", 1000.00m), Is.True);
            });
=======
  
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
>>>>>>> origin/master
        }
    }
}