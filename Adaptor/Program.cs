using System;
using Adaptor.Factories;
using Adaptor.Interfaces;
using Adaptor.Logging;
using Adaptor.Models;
using Adaptor.PaymentProcessors;

namespace Adaptor
{
    /// <summary>
    /// Main program demonstrating the adapter pattern with our payment system
    /// </summary>
    public class Program
    {
        private readonly ILogger _logger = new ConsoleLogger();
        private readonly PaymentProcessorFactory _paymentFactory;
        private readonly ShoppingCart _cart;

        /// <summary>
        /// Constructor for the Program class
        /// </summary>
        public Program()
        {
            _logger = new ConsoleLogger();
            _paymentFactory = new PaymentProcessorFactory(_logger);
            _cart = new ShoppingCart(_logger);
        }

        /// <summary>
        /// Main entry point for the application
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                var program = new Program();
                program.Run();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Run the main application logic
        /// </summary>
        public void Run()
        {
            DisplayWelcomeScreen();
            InitializeShoppingCart();
            RunMainMenu();
        }

        /// <summary>
        /// Display the welcome screen
        /// </summary>
        private void DisplayWelcomeScreen()
        {
            Console.Clear();
            _logger.LogInfo("===========================================");
            _logger.LogInfo("      WEBSHOP DEMO - ADAPTER PATTERN      ");
            _logger.LogInfo("===========================================\n");
            _logger.LogInfo("Dit programma demonstreert het Adapter Pattern");
            _logger.LogInfo("waarbij verschillende betalingssystemen uniform");
            _logger.LogInfo("worden aangestuurd via één interface.\n");
            _logger.LogInfo("Druk op een toets om verder te gaan...");
            Console.ReadKey();
        }

        /// <summary>
        /// Initialize the shopping cart with sample items
        /// </summary>
        private void InitializeShoppingCart()
        {
            _cart.AddItem("Smartphone", 599.99m);
            _cart.AddItem("Beschermhoes", 29.95m);
            _cart.AddItem("Screenprotector", 9.99m);
        }

        /// <summary>
        /// Run the main menu loop
        /// </summary>
        private void RunMainMenu()
        {
            bool exitRequested = false;

            while (!exitRequested)
            {
                DisplayMainMenu();
                string choice = Console.ReadLine() ?? string.Empty;
                exitRequested = ProcessMenuChoice(choice);
            }

            _logger.LogSuccess("\nBedankt voor het winkelen! Tot ziens!");
        }

        /// <summary>
        /// Display the main menu options
        /// </summary>
        private void DisplayMainMenu()
        {
            Console.Clear();
            _logger.LogInfo("\n==== HOOFDMENU ====");
            _logger.LogInfo("1. Bekijk winkelwagen");
            _logger.LogInfo("2. Afrekenen met creditcard");
            _logger.LogInfo("3. Afrekenen met PayPal");
            _logger.LogInfo("4. Demo refund proces");
            _logger.LogInfo("5. Over het Adapter Pattern");
            _logger.LogInfo("6. Afsluiten");
            _logger.LogInfo("\nMaak uw keuze (1-6): ");
        }

        /// <summary>
        /// Process the user's menu choice
        /// </summary>
        /// <param name="choice">The user's choice</param>
        /// <returns>True if the program should exit, false otherwise</returns>
        private bool ProcessMenuChoice(string choice)
        {
            string customerId = "CUST001";

            switch (choice)
            {
                case "1":
                    _cart.DisplayCart();
                    PauseAndContinue();
                    return false;

                case "2":
                    // Use credit card payment
                    var ccProcessor = _paymentFactory.CreatePaymentProcessor(PaymentProcessorFactory.PaymentMethod.CreditCard);
                    _cart.Checkout(ccProcessor, customerId);
                    PauseAndContinue();
                    return false;

                case "3":
                    // Use PayPal via the adapter
                    var paypalProcessor = _paymentFactory.CreatePaymentProcessor(PaymentProcessorFactory.PaymentMethod.PayPal);
                    _cart.Checkout(paypalProcessor, customerId);
                    PauseAndContinue();
                    return false;

                case "4":
                    // Demo a refund process
                    DemoRefundProcess();
                    PauseAndContinue();
                    return false;
                
                case "5":
                    ShowPatternExplanation();
                    PauseAndContinue();
                    return false;

                case "6":
                    return true;

                default:
                    _logger.LogWarning("\nOngeldige keuze. Probeer opnieuw.");
                    PauseAndContinue();
                    return false;
            }
        }

        /// <summary>
        /// Show an explanation of the Adapter pattern
        /// </summary>
        private void ShowPatternExplanation()
        {
            Console.Clear();
            _logger.LogInfo("=== HET ADAPTER PATTERN ===");
            _logger.LogInfo("\nDoel:");
            _logger.LogInfo("Het Adapter Pattern converteert de interface van een klasse");
            _logger.LogInfo("naar een andere interface die de client verwacht.");
            
            _logger.LogInfo("\nDelen in ons voorbeeld:");
            _logger.LogInfo("- Target: IPaymentProcessor (de interface die we willen gebruiken)");
            _logger.LogInfo("- Adaptee: PayPalService (de externe service met een andere interface)");
            _logger.LogInfo("- Adapter: PayPalAdapter (maakt PayPalService compatibel met IPaymentProcessor)");
            _logger.LogInfo("- Client: ShoppingCart (gebruikt IPaymentProcessor zonder te weten hoe PayPal werkt)");
            
            _logger.LogInfo("\nVoordelen:");
            _logger.LogInfo("1. Hergebruik van bestaande code zonder aanpassing");
            _logger.LogInfo("2. Flexibiliteit om nieuwe systemen te integreren zonder client code te wijzigen");
            _logger.LogInfo("3. Duidelijke scheiding van verantwoordelijkheden");
        }

        /// <summary>
        /// Demo the refund process for both payment methods
        /// </summary>
        private void DemoRefundProcess()
        {
            Console.Clear();
            _logger.LogInfo("\n===== REFUND DEMO =====");
            _logger.LogInfo("1. Creditcard refund");
            _logger.LogInfo("2. PayPal refund");
            _logger.LogInfo("\nMaak uw keuze (1-2): ");

            string choice = Console.ReadLine() ?? string.Empty;

            // Demo transaction IDs
            string ccTransactionId = "CC" + Guid.NewGuid().ToString().Substring(0, 8);
            string paypalTransactionId = "TXN" + Guid.NewGuid().ToString().Substring(0, 8);

            switch (choice)
            {
                case "1":
                    var ccProcessor = new CreditCardPayment(_logger);
                    // First process a payment to have a valid transaction (in a real scenario)
                    _logger.LogInfo("\nFirst making a payment to demonstrate a refund:\n");
                    ccProcessor.ProcessPayment("CUST001", 39.95m);
                    
                    _logger.LogInfo("\nNow processing refund:\n");
                    ccProcessor.RefundPayment(ccTransactionId, 39.95m);
                    break;

                case "2":
                    // For demo purposes, create a new PayPalAdapter
                    var paypalAdapter = new PayPalAdapter(logger: _logger);

                    // First process a payment
                    _logger.LogInfo("\nFirst making a payment to demonstrate a refund:\n");
                    paypalAdapter.ProcessPayment("CUST002", 149.95m);

                    _logger.LogInfo("\nNow processing refund:\n");
                    paypalAdapter.RefundPayment(paypalTransactionId, 49.95m);
                    break;

                default:
                    _logger.LogWarning("\nOngeldige keuze.");
                    break;
            }
        }

        /// <summary>
        /// Pause and wait for the user to continue
        /// </summary>
        private void PauseAndContinue()
        {
            _logger.LogInfo("\nDruk op een toets om terug te keren naar het hoofdmenu...");
            Console.ReadKey();
        }
    }
}