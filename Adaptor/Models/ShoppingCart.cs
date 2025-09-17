using Adaptor.Interfaces;
using Adaptor.Logging;
using System;
using System.Collections.Generic;

namespace Adaptor.Models
{
    /// <summary>
    /// A simple shopping cart class to demo our payment processors
    /// </summary>
    public class ShoppingCart
    {
        private readonly List<CartItem> _items = new();
        private readonly ILogger _logger;
        
        /// <summary>
        /// Constructor for the ShoppingCart
        /// </summary>
        /// <param name="logger">Logger instance (optional)</param>
        public ShoppingCart(ILogger? logger = null)
        {
            _logger = logger ?? new ConsoleLogger();
        }
        
        /// <summary>
        /// Add an item to the shopping cart
        /// </summary>
        /// <param name="itemName">The name of the item</param>
        /// <param name="price">The price of the item</param>
        public void AddItem(string itemName, decimal price)
        {
            _items.Add(new CartItem
            {
                Name = itemName,
                Price = price
            });
            
            _logger.LogInfo($"Added {itemName} (€{price:F2}) to cart");
        }

        /// <summary>
        /// Get the total price of all items in the cart
        /// </summary>
        /// <returns>The total price</returns>
        public decimal GetTotal()
        {
            decimal total = 0m;
            
            foreach (var item in _items)
            {
                total += item.Price;
            }
            
            return total;
        }

        /// <summary>
        /// Display the contents of the cart
        /// </summary>
        public void DisplayCart()
        {
            Console.WriteLine("\n===== WINKELWAGEN =====");
            
            if (_items.Count == 0)
            {
                _logger.LogInfo("De winkelwagen is leeg");
                Console.WriteLine("======================\n");
                return;
            }
            
            foreach (var item in _items)
            {
                _logger.LogInfo($"- {item.Name} (€{item.Price:F2})");
            }
            
            _logger.LogInfo($"Totaal: €{GetTotal():F2}");
            Console.WriteLine("======================\n");
        }

        /// <summary>
        /// Process the checkout using the specified payment processor
        /// </summary>
        /// <param name="paymentProcessor">The payment processor to use</param>
        /// <param name="customerId">The customer ID</param>
        /// <returns>True if the checkout was successful, false otherwise</returns>
        public bool Checkout(IPaymentProcessor paymentProcessor, string customerId)
        {
            if (_items.Count == 0)
            {
                _logger.LogWarning("Cannot checkout an empty cart");
                return false;
            }
            
            _logger.LogInfo("\n===== AFREKENEN =====");
            DisplayCart();

            decimal total = GetTotal();
            bool success = paymentProcessor.ProcessPayment(customerId, total);

            if (success)
            {
                _items.Clear();
                _logger.LogSuccess("Bedankt voor je bestelling!");
                return true;
            }
            else
            {
                _logger.LogError("Er is iets fout gegaan bij het afrekenen. Probeer het opnieuw.");
                return false;
            }
        }
    }
    
    /// <summary>
    /// Class representing an item in the shopping cart
    /// </summary>
    public class CartItem
    {
        /// <summary>
        /// The name of the item
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// The price of the item
        /// </summary>
        public decimal Price { get; set; }
    }
}