using Adaptor.Interfaces;
using Adaptor.PaymentProcessors;

namespace Adaptor.Tests
{
    [TestFixture]
    public class PaymentProcessorInterfaceTests
    {
        private PayPalAdapter _paypalAdapter;
        private CreditCardPayment _creditCardPayment;
        
        [SetUp]
        public void Setup()
        {
            _paypalAdapter = new PayPalAdapter();
            _creditCardPayment = new CreditCardPayment();
        }
        
        [Test]
        public void BothProcessors_ImplementInterface()
        {
            IPaymentProcessor ccProcessor = _creditCardPayment;
            IPaymentProcessor ppProcessor = _paypalAdapter;
            
            Assert.That(ccProcessor.ProcessPayment("TEST1", 100.00m), Is.True);
            Assert.That(ppProcessor.ProcessPayment("TEST2", 100.00m), Is.True);
        }
        
        [Test]
        public void BothProcessors_HaveSameInterfaceMethods()
        {
            Assert.That(_creditCardPayment.GetType().GetInterfaces(), Does.Contain(typeof(IPaymentProcessor)));
            Assert.That(_paypalAdapter.GetType().GetInterfaces(), Does.Contain(typeof(IPaymentProcessor)));
        }
    }
}