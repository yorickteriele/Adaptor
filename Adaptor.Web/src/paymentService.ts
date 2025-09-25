import { PaymentRequest, PaymentResponse } from './Payment';

const API_BASE_URL = 'http://localhost:5079/api';

export const paymentApi = {
  async processPayment(payment: PaymentRequest): Promise<PaymentResponse> {
    const response = await fetch(`${API_BASE_URL}/payment/process`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(payment),
    });

    if (!response.ok) {
      throw new Error(`Payment failed: ${response.statusText}`);
    }

    return response.json();
  },

  async getSupportedMethods(): Promise<string[]> {
    const response = await fetch(`${API_BASE_URL}/payment/methods`);
    
    if (!response.ok) {
      throw new Error(`Failed to get payment methods: ${response.statusText}`);
    }

    return response.json();
  },

  async getHealthCheck(): Promise<any> {
    const response = await fetch(`${API_BASE_URL}/payment/health`);
    
    if (!response.ok) {
      throw new Error(`Health check failed: ${response.statusText}`);
    }

    return response.json();
  }
};