using Microsoft.Extensions.Configuration;
using RoseApp.Application.Contracts.IServices;
using RoseApp.Application.DTOs.Payments;
using Stripe;

namespace RoseApp.Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private readonly IConfiguration _configuration;

    public PaymentService(IConfiguration configuration)
    {
        _configuration = configuration;
        StripeConfiguration.ApiKey = configuration["StripeSettings:SecretKey"];
    }

    public async Task<PaymentIntentDto> CreatePaymentIntentAsync(
        Guid orderId,
        decimal amount)
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(amount * 100), 
            Currency = "usd",
            Metadata = new Dictionary<string, string>
            {
                { "orderId", orderId.ToString() }
            }
        };

        var service = new PaymentIntentService();
        var paymentIntent = await service.CreateAsync(options);

        return new PaymentIntentDto
        {
            PaymentIntentId = paymentIntent.Id,
            ClientSecret = paymentIntent.ClientSecret,
            Amount = amount,
            Currency = "usd"
        };
    }

    public async Task<bool> ConfirmPaymentAsync(string paymentIntentId)
    {
        var service = new PaymentIntentService();
        var paymentIntent = await service.GetAsync(paymentIntentId);

        return paymentIntent.Status == "succeeded";
    }

    public async Task<bool> RefundPaymentAsync(string paymentIntentId)
    {
        var service = new PaymentIntentService();
        var paymentIntent = await service.GetAsync(paymentIntentId);

        var refundOptions = new RefundCreateOptions
        {
            PaymentIntent = paymentIntentId,
            Amount = paymentIntent.Amount
        };

        var refundService = new RefundService();
        var refund = await refundService.CreateAsync(refundOptions);

        return refund.Status == "succeeded";
    }

    public bool VerifyWebhookSignature(
        string payload,
        string signature,
        out string eventType,
        out string paymentIntentId)
    {
        eventType = string.Empty;
        paymentIntentId = string.Empty;

        try
        {
            var webhookSecret = _configuration["StripeSettings:WebhookSecret"]!;
            var stripeEvent = EventUtility.ConstructEvent(payload, signature, webhookSecret);

            eventType = stripeEvent.Type;

            if (stripeEvent.Data.Object is PaymentIntent paymentIntent)
                paymentIntentId = paymentIntent.Id;

            return true;
        }
        catch
        {
            return false;
        }
    }
}