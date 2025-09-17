using Adaptor.Interfaces;
using Adaptor.Logging;
using Adaptor.PaymentProcessors;

namespace Adaptor.Factories
{
    /// <summary>
    /// Factory class for creating payment processors.
    /// This demonstrates how we can use different payment processors interchangeably.
    /// </summary>
    public class PaymentProcessorFactory
    {
       
        public enum PaymentMethod
        {
            CreditCard,
            PayPal
        }
        
        private readonly ILogger _logger;
        
        public PaymentProcessorFactory(ILogger? logger = null)
        {
            _logger = logger ?? new ConsoleLogger();
        }
        
        public IPaymentProcessor CreatePaymentProcessor(PaymentMethod method)
        {
            switch (method)
            {
                case PaymentMethod.CreditCard:
                    _logger.LogInfo("Creating Credit Card payment processor");
                    return new CreditCardPayment(_logger);
                    
                case PaymentMethod.PayPal:
                    _logger.LogInfo("Creating PayPal payment processor");
                    return new PayPalAdapter(logger: _logger);
                    
                default:
                    string errorMessage = $"Unsupported payment method: {method}";
                    _logger.LogError(errorMessage);
                    throw new ArgumentException(errorMessage);
            }
        }
    }
}