using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Contracts.IServices;
using RoseApp.Application.DTOs.Payments;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Commerce;
using RoseApp.Domain.Enums;

namespace RoseApp.Application.Features.Payments.Commands.CreatePaymentIntent;

public class CreatePaymentIntentCommandHandler
    : IRequestHandler<CreatePaymentIntentCommand, ResponseViewModel<PaymentIntentDto>>
{
    private readonly IDbContext _context;
    private readonly IPaymentService _paymentService;

    public CreatePaymentIntentCommandHandler(
        IDbContext context,
        IPaymentService paymentService)
    {
        _context = context;
        _paymentService = paymentService;
    }

    public async Task<ResponseViewModel<PaymentIntentDto>> Handle(
        CreatePaymentIntentCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _context.Set<Order>()
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order is null)
            throw new NotFoundException(nameof(Order), request.OrderId);

        if (order.Status != OrderStatus.Pending)
            return ResponseViewModel<PaymentIntentDto>.Fail(
                "Order is not in a payable state", 400);

        var existingPayment = await _context.Set<Payment>()
            .FirstOrDefaultAsync(p => p.OrderId == request.OrderId, cancellationToken);

        if (existingPayment is not null
            && existingPayment.Status == PaymentStatus.Succeeded)
            return ResponseViewModel<PaymentIntentDto>.Fail(
                "Order is already paid", 400);

        var paymentIntentDto = await _paymentService
            .CreatePaymentIntentAsync(order.Id, order.FinalPrice);

        if (existingPayment is null)
        {
            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                StripePaymentIntentId = paymentIntentDto.PaymentIntentId,
                Amount = order.FinalPrice,
                Status = PaymentStatus.Pending
            };

            await _context.Set<Payment>().AddAsync(payment, cancellationToken);
        }
        else
        {
            existingPayment.StripePaymentIntentId = paymentIntentDto.PaymentIntentId;
            existingPayment.Status = PaymentStatus.Pending;
            _context.Entry(existingPayment).State = EntityState.Modified;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<PaymentIntentDto>.Success(paymentIntentDto);
    }
}