using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Orders;
using RoseApp.Domain.Entities.Commerce;
using RoseApp.Domain.Entities.UserDomain;

namespace RoseApp.Application.Features.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler
    : IRequestHandler<GetOrdersQuery, ResponseViewModel<PaginationResult<OrderDto>>>
{
    private readonly IDbContext _context;

    public GetOrdersQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<PaginationResult<OrderDto>>> Handle(
        GetOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Set<Order>()
            .AsNoTracking()
            .Where(o => o.UserId == request.UserId);

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(o =>
                o.Notes != null &&
                o.Notes.Contains(request.Search));

        var totalCount = await query.CountAsync(cancellationToken);

        var orders = await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((request.Page - 1) * request.Limit)
            .Take(request.Limit)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                Status = o.Status.ToString(),
                TotalPrice = o.TotalPrice,
                DiscountAmount = o.DiscountAmount,
                FinalPrice = o.FinalPrice,
                Notes = o.Notes,
                CreatedAt = o.CreatedAt,
                Address = new AddressInfoDto
                {
                    Label = o.Address.Label,
                    Street = o.Address.Street,
                    City = o.Address.City,
                    Country = o.Address.Country
                },
                Coupon = o.Coupon == null ? null : new CouponInfoDto
                {
                    Code = o.Coupon.Code,
                    DiscountAmount = o.DiscountAmount
                },
                Items = o.Items.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    ProductPhoto = i.Product.PhotoUrl,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice
                }).ToList()
            })
            .ToListAsync(cancellationToken);

        var result = PaginationResult<OrderDto>.Create(
            orders, totalCount, request.Page, request.Limit);

        return ResponseViewModel<PaginationResult<OrderDto>>.Success(result);
    }
}