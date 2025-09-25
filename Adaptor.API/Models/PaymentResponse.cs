namespace Adaptor.API.Models
{
    public class PaymentResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = "";
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = "";
        public string TransactionId { get; set; } = "";
        public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
    }
}