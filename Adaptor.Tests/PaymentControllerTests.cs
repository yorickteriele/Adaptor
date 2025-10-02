using Adaptor.API.Controllers;
using Adaptor.API.Models;
using Adaptor.API.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

namespace Adaptor.Tests;

[TestFixture]
public class PaymentControllerTests
{
    private PaymentController _controller;
    private ServiceProvider _serviceProvider;

    [SetUp]
    public void Setup()
    {
        var services = new ServiceCollection();
        services.AddTransient<CreditCardPayment>();
        services.AddTransient<PayPalService>();
        services.AddTransient<PayPalAdapter>();
        _serviceProvider = services.BuildServiceProvider();
        _controller = new PaymentController(_serviceProvider);
    }

    [TearDown]
    public void TearDown()
    {
        _serviceProvider?.Dispose();
    }

    [Test]
    public async Task ProcessPayment_WithCreditCard_ShouldRouteCorrectly()
    {
        var request = new PaymentRequest
        {
            Amount = 200.00m,
            PaymentMethod = "creditcard",
            CardNumber = "4111111111111111",
            CardHolderName = "Jane Smith"
        };

        var result = await _controller.ProcessPayment(request);

        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        var response = ((OkObjectResult)result.Result).Value as PaymentResponse;
        Assert.That(response!.IsSuccess, Is.True);
        Assert.That(response.PaymentMethod, Is.EqualTo("CreditCard"));
    }

    [Test]
    public async Task ProcessPayment_WithPayPal_ShouldRouteCorrectly()
    {
        var request = new PaymentRequest
        {
            Amount = 300.00m,
            PaymentMethod = "paypal",
            PayPalEmail = "customer@example.com"
        };

        var result = await _controller.ProcessPayment(request);

        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        var response = ((OkObjectResult)result.Result).Value as PaymentResponse;
        Assert.That(response!.IsSuccess, Is.True);
        Assert.That(response.PaymentMethod, Is.EqualTo("PayPal"));
    }

    [Test]
    public async Task ProcessPayment_WithUnsupportedMethod_ShouldReturnBadRequest()
    {
        var request = new PaymentRequest
        {
            Amount = 100.00m,
            PaymentMethod = "bitcoin"
        };

        var result = await _controller.ProcessPayment(request);

        Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        var response = ((BadRequestObjectResult)result.Result).Value as PaymentResponse;
        Assert.That(response!.IsSuccess, Is.False);
        Assert.That(response.Message, Is.EqualTo("Unsupported payment method"));
    }

    [Test]
    public async Task ProcessPayment_WithInvalidAmount_ShouldReturnBadRequest()
    {
        var request = new PaymentRequest
        {
            Amount = 0,
            PaymentMethod = "creditcard"
        };

        var result = await _controller.ProcessPayment(request);

        Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        var response = ((BadRequestObjectResult)result.Result).Value as PaymentResponse;
        Assert.That(response!.IsSuccess, Is.False);
        Assert.That(response.Message, Is.EqualTo("Invalid amount"));
    }

    [Test]
    public void HealthCheck_ShouldReturnOkWithAdapterPattern()
    {
        var result = _controller.HealthCheck();
        
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        var response = ((OkObjectResult)result.Result).Value!.ToString();
        Assert.That(response, Does.Contain("Status = OK"));
        Assert.That(response, Does.Contain("Pattern = Adapter"));
    }
}