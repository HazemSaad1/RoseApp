using RoseApp.Application.DTOs.Payments;

namespace RoseApp.Application.Contracts.IServices;

public interface IPaymentService
{
    Task<PaymentIntentDto> CreatePaymentIntentAsync(Guid orderId, decimal amount);
    Task<bool> ConfirmPaymentAsync(string paymentIntentId);
    Task<bool> RefundPaymentAsync(string paymentIntentId);
    bool VerifyWebhookSignature(string payload, string signature, out string eventType, out string paymentIntentId);
}