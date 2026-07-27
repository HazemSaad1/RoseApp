using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Contracts.IServices;
using RoseApp.Domain.Entities.Commerce;
using RoseApp.Domain.Enums;

namespace RoseApp.Application.Features.Payments.Commands.RefundPayment;

public class RefundPaymentCommandHandler
    : IRequestHandler<RefundPaymentCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;
    private readonly IPaymentService _paymentService;

    public RefundPaymentCommandHandler(
        IDbContext context,
        IPaymentService paymentService)
    {
        _context = context;
        _paymentService = paymentService;
    }

    public async Task<ResponseViewModel<string>> Handle(
        RefundPaymentCommand request,
        CancellationToken cancellationToken)
    {
        var payment = await _context.Set<Payment>()
            .FirstOrDefaultAsync(p =>
                p.StripePaymentIntentId == request.PaymentIntentId,
                cancellationToken);

        if (payment is null)
            return ResponseViewModel<string>.Fail("Payment not found", 404);

        if (payment.Status != PaymentStatus.Succeeded)
            return ResponseViewModel<string>.Fail(
                "Only succeeded payments can be refunded", 400);

        var isRefunded = await _paymentService.RefundPaymentAsync(
            request.PaymentIntentId);

        if (!isRefunded)
            return ResponseViewModel<string>.Fail("Refund failed", 400);

        payment.Status = PaymentStatus.Refunded;
        _context.Entry(payment).State = EntityState.Modified;

        var order = await _context.Set<Order>()
            .FirstOrDefaultAsync(o => o.Id == payment.OrderId, cancellationToken);

        if (order is not null)
        {
            order.Status = OrderStatus.Cancelled;
            _context.Entry(order).State = EntityState.Modified;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Payment refunded successfully");
    }
}