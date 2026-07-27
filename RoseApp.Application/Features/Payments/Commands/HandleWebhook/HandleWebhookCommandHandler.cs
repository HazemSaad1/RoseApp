using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Contracts.IServices;
using RoseApp.Domain.Entities.Commerce;
using RoseApp.Domain.Enums;

namespace RoseApp.Application.Features.Payments.Commands.HandleWebhook;

public class HandleWebhookCommandHandler
    : IRequestHandler<HandleWebhookCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;
    private readonly IPaymentService _paymentService;

    public HandleWebhookCommandHandler(
        IDbContext context,
        IPaymentService paymentService)
    {
        _context = context;
        _paymentService = paymentService;
    }

    public async Task<ResponseViewModel<string>> Handle(
        HandleWebhookCommand request,
        CancellationToken cancellationToken)
    {
        var isValid = _paymentService.VerifyWebhookSignature(
            request.Payload,
            request.Signature,
            out var eventType,
            out var paymentIntentId);

        if (!isValid)
            return ResponseViewModel<string>.Fail("Invalid webhook signature", 400);

        var payment = await _context.Set<Payment>()
            .FirstOrDefaultAsync(p =>
                p.StripePaymentIntentId == paymentIntentId,
                cancellationToken);

        if (payment is null)
            return ResponseViewModel<string>.Success("Webhook received");

        switch (eventType)
        {
            case "payment_intent.succeeded":
                payment.Status = PaymentStatus.Succeeded;

                var order = await _context.Set<Order>()
                    .FirstOrDefaultAsync(o => o.Id == payment.OrderId,
                        cancellationToken);

                if (order is not null)
                    order.Status = OrderStatus.Confirmed;
                break;

            case "payment_intent.payment_failed":
                payment.Status = PaymentStatus.Failed;
                break;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Webhook processed successfully");
    }
}