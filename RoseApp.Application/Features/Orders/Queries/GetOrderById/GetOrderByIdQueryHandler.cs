using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Orders;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Commerce;

namespace RoseApp.Application.Features.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryHandler
    : IRequestHandler<GetOrderByIdQuery, ResponseViewModel<OrderDto>>
{
    private readonly IDbContext _context;

    public GetOrderByIdQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<OrderDto>> Handle(
        GetOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        var order = await _context.Set<Order>()
            .Include(o => o.Address)
            .Include(o => o.Coupon)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order is null)
            throw new NotFoundException(nameof(Order), request.OrderId);

        if (order.UserId != request.UserId)
            return ResponseViewModel<OrderDto>.Fail("Unauthorized", 403);

        var dto = new OrderDto
        {
            Id = order.Id,
            Status = order.Status.ToString(),
            TotalPrice = order.TotalPrice,
            DiscountAmount = order.DiscountAmount,
            FinalPrice = order.FinalPrice,
            Notes = order.Notes,
            CreatedAt = order.CreatedAt,
            Address = new AddressInfoDto
            {
                Label = order.Address.Label,
                Street = order.Address.Street,
                City = order.Address.City,
                Country = order.Address.Country
            },
            Coupon = order.Coupon == null ? null : new CouponInfoDto
            {
                Code = order.Coupon.Code,
                DiscountAmount = order.DiscountAmount
            },
            Items = order.Items.Select(i => new OrderItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                ProductPhoto = i.Product.PhotoUrl,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                TotalPrice = i.TotalPrice
            }).ToList()
        };

        return ResponseViewModel<OrderDto>.Success(dto);
    }
}