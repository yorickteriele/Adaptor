using Microsoft.AspNetCore.Mvc;
using Adaptor.API.Interfaces;
using Adaptor.API.Models;
using Adaptor.API.Services;

namespace Adaptor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;

        public PaymentController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [HttpPost("process")]
        public async Task<ActionResult<PaymentResponse>> ProcessPayment([FromBody] PaymentRequest request)
        {
            if (request.Amount <= 0)
                return BadRequest(new PaymentResponse 
                { 
                    IsSuccess = false, 
                    Message = "Invalid amount", 
                    Amount = request.Amount, 
                    PaymentMethod = request.PaymentMethod 
                });

            try
            {
                IPaymentProcessor processor = request.PaymentMethod.ToLower() switch
                {
                    "creditcard" => _serviceProvider.GetRequiredService<CreditCardPayment>(),
                    "paypal" => _serviceProvider.GetRequiredService<CreditCardPayment>(),
                    _ => throw new ArgumentException("Unsupported payment method")
                };

                var result = await processor.ProcessPayment(request.Amount, request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new PaymentResponse 
                { 
                    IsSuccess = false, 
                    Message = ex.Message, 
                    Amount = request.Amount, 
                    PaymentMethod = request.PaymentMethod 
                });
            }
        }

        [HttpGet("health")]
        public ActionResult<object> HealthCheck()
        {
            return Ok(new { Status = "OK", Pattern = "Adapter" });
        }
    }
}