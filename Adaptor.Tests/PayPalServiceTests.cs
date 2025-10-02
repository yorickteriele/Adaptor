using Adaptor.API.Services;
using Adaptor.API.Models;

namespace Adaptor.Tests;

[TestFixture]
public class PayPalServiceTests
{
    private PayPalService _payPalService;

    [SetUp]
    public void Setup()
    {
        _payPalService = new PayPalService();
    }

    [Test]
    public async Task SendMoney_WithValidEmail_ShouldCalculateCorrectFee()
    {
        var result = await _payPalService.SendMoney("test@example.com", 100.00m);

        var expectedFee = 100.00m * 0.029m + 0.30m;
        Assert.That(result.Fee, Is.EqualTo(expectedFee));
        Assert.That(result.Success, Is.True);
        Assert.That(result.ProcessedAmount, Is.EqualTo(100.00m));
        Assert.That(result.PayPalTransactionId, Does.StartWith("PP_"));
    }

    [Test]
    public async Task SendMoney_WithInvalidEmail_ShouldFail()
    {
        var result = await _payPalService.SendMoney("invalid-email", 50.00m);

        Assert.That(result.Success, Is.False);
        Assert.That(result.Error, Is.EqualTo("Invalid email"));
        Assert.That(result.PayPalTransactionId, Is.Empty);
    }

    [Test]
    public async Task SendMoney_WithEmptyEmail_ShouldFail()
    {
        var result = await _payPalService.SendMoney("", 25.00m);

        Assert.That(result.Success, Is.False);
        Assert.That(result.Error, Is.EqualTo("Invalid email"));
    }
}