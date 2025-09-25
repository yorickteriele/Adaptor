export interface PaymentRequest {
  amount: number;
  currency: string;
  paymentMethod: string;
  cardNumber?: string;
  expiryDate?: string;
  cvv?: string;
  cardHolderName?: string;
  payPalEmail?: string;
}

export interface PaymentResponse {
  isSuccess: boolean;
  message: string;
  amount: number;
  paymentMethod: string;
  transactionId: string;
  processedAt: string;
}