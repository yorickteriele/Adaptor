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
        /// <summary>
        /// Supported payment methods
        /// </summary>
        public enum PaymentMethod
        {
            /// <summary>
            /// Credit card payment
            /// </summary>
            CreditCard,
            
            /// <summary>
            /// PayPal payment
            /// </summary>
            PayPal
            
            // Add more payment methods here as needed
        }
        
        private readonly ILogger _logger;
        
        /// <summary>
        /// Constructor for the PaymentProcessorFactory
        /// </summary>
        /// <param name="logger">Logger instance (optional)</param>
        public PaymentProcessorFactory(ILogger? logger = null)
        {
            _logger = logger ?? new ConsoleLogger();
        }
        
        /// <summary>
        /// Create a payment processor for the specified payment method
        /// </summary>
        /// <param name="method">The payment method to create a processor for</param>
        /// <returns>An implementation of IPaymentProcessor</returns>
        /// <exception cref="ArgumentException">Thrown if the payment method is not supported</exception>
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