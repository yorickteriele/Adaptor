using Adaptor.API.Models;

namespace Adaptor.API.Services
{
    public class PayPalService
    {
        public async Task<PayPalResult> SendMoney(string email, decimal amount)
        {
            await Task.Delay(800);
            
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                return new PayPalResult 
                { 
                    Success = false, 
                    Error = "Invalid email" 
                };
            }
                
            return new PayPalResult 
            { 
                Success = true, 
                PayPalTransactionId = $"PP_{DateTime.UtcNow.Ticks}", 
                ProcessedAmount = amount,
                Fee = amount * 0.029m + 0.30m
            };
        }
    }
}