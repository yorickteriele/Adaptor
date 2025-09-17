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
        
        [SetUp]
        public void Setup()
        {
            _creditCardPayment = new CreditCardPayment();
        }
        
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
        }
    }
}