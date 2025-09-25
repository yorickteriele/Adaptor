namespace Adaptor.API.Models
{
    public class PayPalResult
    {
        public bool Success { get; set; }
        public string PayPalTransactionId { get; set; } = "";
        public decimal ProcessedAmount { get; set; }
        public decimal Fee { get; set; }
        public string Error { get; set; } = "";
    }
}