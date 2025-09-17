using Adaptor.Interfaces;
using Adaptor.PaymentProcessors;
using Adaptor.ExternalServices;

namespace Adaptor.Tests
{
    [TestFixture]
    public class PayPalAdapterTests
    {
        private PayPalAdapter _paypalAdapter;
        
        [SetUp]
        public void Setup()
        {
            _paypalAdapter = new PayPalAdapter();
        }
        
        [Test]
        public void ProcessPayment_StandardAmount_ReturnsTrue()
        {
            var result = _paypalAdapter.ProcessPayment("CUST456", 75.99m);
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void RefundPayment_NonExistentTransaction_ReturnsFalse()
        {
            var result = _paypalAdapter.RefundPayment("NONEXISTENT", 10.00m);
            Assert.That(result, Is.False);
        }
        
        [Test]
        public void GetPaymentStatus_NonExistentTransaction_ContainsNotFound()
        {
            var status = _paypalAdapter.GetPaymentStatus("TX1234");
            Assert.That(status, Does.Contain("not found"));
        }
        
        [Test]
        public void ProcessPayment_SmallAmount_ReturnsTrue()
        {
            var result = _paypalAdapter.ProcessPayment("CUST999", 0.01m);
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void ProcessPayment_LargeAmount_ReturnsTrue()
        {
            var result = _paypalAdapter.ProcessPayment("CUST999", 9999.99m);
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void ProcessAndGetStatus_ReturnsNonEmptyStatus()
        {
            _paypalAdapter.ProcessPayment("CUST789", 150.00m);
            var status = _paypalAdapter.GetPaymentStatus("TX12345");
            Assert.That(status, Is.Not.Empty);
        }
        
        [Test]
        public void ProcessPayment_VariousAmounts_AllSucceed()
        {
            Assert.Multiple(() => {
                Assert.That(_paypalAdapter.ProcessPayment("CUST4", 10.00m), Is.True);
                Assert.That(_paypalAdapter.ProcessPayment("CUST5", 99.99m), Is.True);
                Assert.That(_paypalAdapter.ProcessPayment("CUST6", 1000.00m), Is.True);
            });
        }
    }
}