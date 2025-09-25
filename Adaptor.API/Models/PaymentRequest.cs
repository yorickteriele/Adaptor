namespace Adaptor.API.Models
{
    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EUR";
        public string PaymentMethod { get; set; } = "";
        
        public string? CardNumber { get; set; }
        public string? ExpiryDate { get; set; }
        public string? Cvv { get; set; }
        public string? CardHolderName { get; set; }
        
        public string? PayPalEmail { get; set; }
    }
}