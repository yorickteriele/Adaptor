using Adaptor.API.Services;
using Adaptor.API.Models;

namespace Adaptor.Tests;

[TestFixture]
public class PayPalAdapterTests
{
    private PayPalAdapter _payPalAdapter;

    [SetUp]
    public void Setup()
    {
        _payPalAdapter = new PayPalAdapter(new PayPalService());
    }

    [Test]
    public async Task ProcessPayment_WithValidEmail_ShouldSucceed()
    {
        var request = new PaymentRequest
        {
            Amount = 150.00m,
            PaymentMethod = "PayPal",
            PayPalEmail = "test@example.com"
        };

        var result = await _payPalAdapter.ProcessPayment(150.00m, request);

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.PaymentMethod, Is.EqualTo("PayPal"));
        Assert.That(result.TransactionId, Does.StartWith("PP_"));
        Assert.That(result.Message, Does.StartWith("PayPal payment successful"));
    }

    [Test]
    public async Task ProcessPayment_WithMissingEmail_ShouldFail()
    {
        var request = new PaymentRequest
        {
            Amount = 150.00m,
            PaymentMethod = "PayPal",
            PayPalEmail = ""
        };

        var result = await _payPalAdapter.ProcessPayment(150.00m, request);

        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Message, Is.EqualTo("PayPal email required"));
    }

    [Test]
    public async Task ProcessPayment_WithInvalidEmail_ShouldFail()
    {
        var request = new PaymentRequest
        {
            Amount = 150.00m,
            PaymentMethod = "PayPal",
            PayPalEmail = "not-an-email"
        };

        var result = await _payPalAdapter.ProcessPayment(150.00m, request);

        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Message, Is.EqualTo("Invalid email"));
    }
}