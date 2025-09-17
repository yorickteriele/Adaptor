namespace Adaptor.Models
{
    /// <summary>
    /// Record class for storing transaction information
    /// </summary>
    public class TransactionRecord
    {
        /// <summary>
        /// The unique identifier for the customer
        /// </summary>
        public string CustomerId { get; set; } = string.Empty;
        
        /// <summary>
        /// The amount of the transaction
        /// </summary>
        public decimal Amount { get; set; }
        
        /// <summary>
        /// The timestamp when the transaction was processed
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// The current status of the transaction (e.g., COMPLETED, PENDING, REFUNDED)
        /// </summary>
        public string Status { get; set; } = string.Empty;
        
        /// <summary>
        /// The amount that was refunded, if any
        /// </summary>
        public decimal? RefundAmount { get; set; }
        
        /// <summary>
        /// The timestamp when the refund was processed, if any
        /// </summary>
        public DateTime? RefundTimestamp { get; set; }
    }
}