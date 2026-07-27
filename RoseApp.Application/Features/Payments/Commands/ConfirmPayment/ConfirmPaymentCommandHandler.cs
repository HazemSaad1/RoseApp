using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Contracts.IServices;
using RoseApp.Domain.Entities.Commerce;
using RoseApp.Domain.Enums;

namespace RoseApp.Application.Features.Payments.Commands.ConfirmPayment;

public class ConfirmPaymentCommandHandler
    : IRequestHandler<ConfirmPaymentCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;
    private readonly IPaymentService _paymentService;

    public ConfirmPaymentCommandHandler(
        IDbContext context,
        IPaymentService paymentService)
    {
        _context = context;
        _paymentService = paymentService;
    }

    public async Task<ResponseViewModel<string>> Handle(
        ConfirmPaymentCommand request,
        CancellationToken cancellationToken)
    {
        var payment = await _context.Set<Payment>()
            .FirstOrDefaultAsync(p =>
                p.StripePaymentIntentId == request.PaymentIntentId,
                cancellationToken);

        if (payment is null)
            return ResponseViewModel<string>.Fail("Payment not found", 404);

        var isSucceeded = await _paymentService.ConfirmPaymentAsync(
            request.PaymentIntentId);

        if (!isSucceeded)
            return ResponseViewModel<string>.Fail("Payment confirmation failed", 400);

        payment.Status = PaymentStatus.Succeeded;
        _context.Entry(payment).State = EntityState.Modified;

        var order = await _context.Set<Order>()
            .FirstOrDefaultAsync(o => o.Id == payment.OrderId, cancellationToken);

        if (order is not null)
        {
            order.Status = OrderStatus.Confirmed;
            _context.Entry(order).State = EntityState.Modified;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Payment confirmed successfully");
    }
}