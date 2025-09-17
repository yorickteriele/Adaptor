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
      
        public ShoppingCart(ILogger? logger = null)
        {
            _logger = logger ?? new ConsoleLogger();
        }
        
        public void AddItem(string itemName, decimal price)
        {
            _items.Add(new CartItem
            {
                Name = itemName,
                Price = price
            });
            
            _logger.LogInfo($"Added {itemName} (€{price:F2}) to cart");
        }

        public decimal GetTotal()
        {
            decimal total = 0m;
            
            foreach (var item in _items)
            {
                total += item.Price;
            }
            
            return total;
        }

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
    
    public class CartItem
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}