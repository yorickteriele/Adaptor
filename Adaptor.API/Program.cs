using Adaptor.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddScoped<CreditCardPayment>();
builder.Services.AddScoped<PayPalService>();

var app = builder.Build();

app.UseCors();
app.MapControllers();

app.Run();
