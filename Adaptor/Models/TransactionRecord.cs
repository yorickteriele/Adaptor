namespace Adaptor.Models
{
    /// <summary>
    /// Record class for storing transaction information
    /// </summary>
    public class TransactionRecord
    {
      
        public string CustomerId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal? RefundAmount { get; set; }
        public DateTime? RefundTimestamp { get; set; }
    }
}