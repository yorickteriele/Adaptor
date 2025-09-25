import React, { useState } from 'react';
import './App.css';

const CART = [
  { name: "Onzichtbare Inkt Pen", price: 23.99 },
  { name: "Draadloze Springstokkabel", price: 26.01 }
];

function App() {
  const [paymentMethod, setPaymentMethod] = useState<'card' | 'paypal'>('card');
  const [loading, setLoading] = useState(false);
  const [result, setResult] = useState<string | null>(null);
  
  const total = CART.reduce((sum, item) => sum + item.price, 0);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    
    await new Promise(resolve => setTimeout(resolve, 1500));
    
    setResult(Math.random() > 0.2 ? 'Betaling geslaagd!' : 'Betaling mislukt');
    setLoading(false);
  };

  if (result) {
    return (
      <div className="app">
        <div className="card">
          <h2>{result}</h2>
          <button onClick={() => setResult(null)}>Nieuwe betaling</button>
        </div>
      </div>
    );
  }

  return (
    <div className="app">
      <div className="card">
        <h1>Checkout</h1>
        
        <div className="cart">
          {CART.map((item, i) => (
            <div key={i} className="cart-item">
              <span>{item.name}</span>
              <span>€{item.price}</span>
            </div>
          ))}
          <div className="total">
            <strong>Totaal: €{total.toFixed(2)}</strong>
          </div>
        </div>

        <form onSubmit={handleSubmit}>
          <div className="payment-methods">
            <label>
              <input 
                type="radio" 
                value="card" 
                checked={paymentMethod === 'card'}
                onChange={(e) => setPaymentMethod('card')}
              />
              Creditcard
            </label>
            <label>
              <input 
                type="radio" 
                value="paypal" 
                checked={paymentMethod === 'paypal'}
                onChange={(e) => setPaymentMethod('paypal')}
              />
              PayPal
            </label>
          </div>

          {paymentMethod === 'card' && (
            <div className="form-group">
              <input type="text" placeholder="Kaartnummer" required />
              <div className="form-row">
                <input type="text" placeholder="MM/JJ" required />
                <input type="text" placeholder="CVV" required />
              </div>
              <input type="text" placeholder="Naam" required />
            </div>
          )}

          {paymentMethod === 'paypal' && (
            <div className="form-group">
              <input type="email" placeholder="PayPal email" required />
            </div>
          )}

          <button type="submit" disabled={loading}>
            {loading ? 'Bezig...' : `Betaal €${total.toFixed(2)}`}
          </button>
        </form>
      </div>
    </div>
  );
}

export default App;
