using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Commerce;

namespace RoseApp.Application.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler
    : IRequestHandler<UpdateOrderCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public UpdateOrderCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        UpdateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _context.Set<Order>()
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order is null)
            throw new NotFoundException(nameof(Order), request.OrderId);

        order.Status = request.Status;

        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Order status updated successfully");
    }
}