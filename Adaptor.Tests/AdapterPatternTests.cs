using System;
using Adaptor.Interfaces;
using Adaptor.PaymentProcessors;
using Adaptor.ExternalServices;

namespace Adaptor.Tests 
{
    [TestFixture]
    public class AdapterPatternTests
    {
        [Test]
        public void AdapterPattern_InterfaceConsistency()
        {
            IPaymentProcessor creditCard = new CreditCardPayment();
            IPaymentProcessor payPal = new PayPalAdapter();
            
            Assert.That(creditCard.GetType().GetMethod("ProcessPayment")?.ToString(),
                Is.EqualTo(payPal.GetType().GetMethod("ProcessPayment")?.ToString()));
                
            Assert.That(creditCard.GetType().GetMethod("RefundPayment")?.ToString(),
                Is.EqualTo(payPal.GetType().GetMethod("RefundPayment")?.ToString()));
                
            Assert.That(creditCard.GetType().GetMethod("GetPaymentStatus")?.ToString(),
                Is.EqualTo(payPal.GetType().GetMethod("GetPaymentStatus")?.ToString()));
        }
        
        [Test]
        public void AdapterPattern_PolymorphicUsage()
        {
            Action<IPaymentProcessor> clientCode = (processor) => {
                processor.ProcessPayment("CLIENT-123", 50.0m);
            };
            
            clientCode(new CreditCardPayment());
            clientCode(new PayPalAdapter());
            
            Assert.Pass();
        }
        
        [Test]
        public void AdapterPattern_TransactionLifecycle()
        {
            IPaymentProcessor processor = new PayPalAdapter();
            
            bool paymentResult = processor.ProcessPayment("CUST-ADAPTER", 75.50m);
            Assert.That(paymentResult, Is.True);
            
            string status = processor.GetPaymentStatus("TX12345");
            Assert.That(status, Is.Not.Null);
        }
    }
}