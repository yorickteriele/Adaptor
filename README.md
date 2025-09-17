# 🎯 Adapter Pattern Webshop Demonstratie

## 📚 Overzicht

Dit project demonstreert het **Adapter Pattern** in een realistische webshop scenario. Het toont hoe je incompatibele externe APIs (zoals PayPal) kunt integreren met je bestaande systeem zonder je eigen code aan te passen.

## 🏗️ Architectuur

### Het Probleem
- Onze webshop werkt met een standaard `IPaymentProcessor` interface
- Creditcard betalingen passen perfect bij deze interface
- PayPal API werkt totaal anders (email + bedrag vs. PaymentRequest object)
- We kunnen de PayPal API niet aanpassen (externe dependency)

### De Oplossing: Adapter Pattern
```
Client Code → IPaymentProcessor → [Adapter] → PayPal API
                     ↑
              CreditCardProcessor (directe implementatie)
```

## 🔧 Componenten

### Core Interface
- **`IPaymentProcessor`**: Standaard interface voor alle betaalmethodes
- **`PaymentRequest`**: Unified request model
- **`PaymentResult`**: Unified response model

### Directe Implementatie
- **`CreditCardPaymentProcessor`**: Past perfect bij onze interface

### Externe API (Incompatibel)
- **`PayPalApiClient`**: Simulatie van echte PayPal API
- **`PayPalTransactionResponse`**: PayPal's eigen response formaat
- Werkt met `(email, email, amount, description)` parameters

### Adapter Implementation
- **`PayPalAdapter`**: 🔌 **Het Adapter Pattern in actie!**
  - Implementeert `IPaymentProcessor`
  - Vertaalt `PaymentRequest` naar PayPal API calls
  - Converteer PayPal responses terug naar `PaymentResult`
  - Handelt valuta conversie af (EUR ↔ USD)

### Webshop Context
- **`Product`**: Productcatalogus
- **`ShoppingCartService`**: Winkelwagen beheer
- **`Customer`**: Klantgegevens

## 🚀 Demo Features

### 1. Volledige Webshop Experience
- Product catalogus met categorieën
- Winkelwagen functionaliteit
- Realistische checkout flow

### 2. Adapter Pattern Demonstratie
- Creditcard: Directe implementatie
- PayPal: Via Adapter Pattern
- Beide zien er identiek uit voor de client!

### 3. Interactieve Console UI
- Gebruiksvriendelijke menu's
- Realtime feedback tijdens betalingen
- Duidelijke logging van adapter werking

## 💡 Adapter Pattern Voordelen

1. **Herbruikbaarheid**: Bestaande code hoeft niet aangepast
2. **Flexibiliteit**: Eenvoudig nieuwe betaalmethodes toevoegen
3. **Scheiding van zorgen**: Elke adapter handelt één specifieke integratie af
4. **Testbaarheid**: Adapters kunnen onafhankelijk getest worden

## 🎓 Leerwaarde

### Design Patterns
- **Adapter Pattern**: Hoofdfocus van deze demo
- **Strategy Pattern**: Verschillende betaalmethodes
- **Factory Pattern**: Payment processor instantiëring

### Best Practices
- Clean Architecture principes
- Dependency Injection
- Async/Await patterns
- Exception handling
- Interface segregation

## 🔍 Code Highlights

### Adapter Magic
```csharp
public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
{
    // 🔄 ADAPTER VERTALING
    var amountInUSD = ConvertEurToUsd(request.Amount);
    var customerEmail = request.Email!;
    var description = $"Webshop bestelling: {request.Description}";

    // Roep PayPal API aan met HUN interface
    var paypalResponse = await _paypalClient.SendPaymentAsync(
        senderEmail: customerEmail,
        receiverEmail: _merchantEmail,
        amount: amountInUSD,
        description: description
    );

    // Converteer terug naar ONS formaat
    return ConvertPayPalResponseToPaymentResult(paypalResponse, request.Amount);
}
```

### Client Code (Gebruikt beide hetzelfde!)
```csharp
foreach (var processor in paymentProcessors)
{
    // Voor de client zien creditcard EN PayPal er identiek uit!
    var result = await processor.ProcessPaymentAsync(request);
    // Adapter pattern maakt dit mogelijk! 🎯
}
```

## 🎯 Presentatie Punten

1. **Probleem**: Incompatibele APIs
2. **Oplossing**: Adapter Pattern
3. **Demo**: Live webshop simulatie
4. **Code Review**: Adapter implementatie
5. **Voordelen**: Flexibiliteit en herbruikbaarheid

## 🚦 Getting Started

1. Clone het project
2. Open in Visual Studio/VS Code
3. Run de applicatie
4. Volg de interactieve demo
5. Probeer beide betaalmethodes
6. Bekijk de adapter logging

## 🎨 Demo Flow

1. **Welkom** → Uitleg Adapter Pattern
2. **Producten** → Bekijk catalogus
3. **Winkelwagen** → Voeg producten toe
4. **Checkout** → Kies betaalmethode
5. **Betaling** → Zie adapter in actie!
6. **Pattern Demo** → Directe vergelijking

---

*Deze implementatie is ontworpen voor educatieve doeleinden en demonstreert professionele software development practices en design patterns.*