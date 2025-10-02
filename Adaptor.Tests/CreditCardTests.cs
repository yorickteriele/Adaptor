using Adaptor.API.Services;
using Adaptor.API.Models;

namespace Adaptor.Tests;

[TestFixture]
public class CreditCardPaymentTests
{
    private CreditCardPayment _creditCardProcessor;

    [SetUp]
    public void Setup()
    {
        _creditCardProcessor = new CreditCardPayment();
    }

    [Test]
    public async Task ProcessPayment_WithValidCard_ShouldSucceed()
    {
        var request = new PaymentRequest
        {
            Amount = 100.00m,
            PaymentMethod = "CreditCard",
            CardNumber = "4111111111111111",
            CardHolderName = "John Doe"
        };

        var result = await _creditCardProcessor.ProcessPayment(100.00m, request);

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.PaymentMethod, Is.EqualTo("CreditCard"));
        Assert.That(result.TransactionId, Does.StartWith("CC_"));
        Assert.That(result.Message, Does.Contain("John Doe"));
    }

    [Test]
    public async Task ProcessPayment_WithMissingCardNumber_ShouldFail()
    {
        var request = new PaymentRequest
        {
            Amount = 100.00m,
            PaymentMethod = "CreditCard",
            CardNumber = "",
            CardHolderName = "John Doe"
        };

        var result = await _creditCardProcessor.ProcessPayment(100.00m, request);

        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Message, Is.EqualTo("Card number required"));
    }
}