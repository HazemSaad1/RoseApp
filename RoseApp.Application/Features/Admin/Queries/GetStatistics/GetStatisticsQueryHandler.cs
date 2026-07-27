using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Admin;
using RoseApp.Domain.Entities.Catalog;
using RoseApp.Domain.Entities.Commerce;
using RoseApp.Domain.Entities.Content;
using RoseApp.Domain.Entities.Identity;
using RoseApp.Domain.Entities.UserDomain;
using RoseApp.Domain.Enums;

namespace RoseApp.Application.Features.Admin.Queries.GetStatistics;

public class GetStatisticsQueryHandler
    : IRequestHandler<GetStatisticsQuery, ResponseViewModel<StatisticsDto>>
{
    private readonly IDbContext _context;

    public GetStatisticsQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<StatisticsDto>> Handle(
        GetStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        var totalUsers = await _context.Set<User>()
            .CountAsync(cancellationToken);

        var totalProducts = await _context.Set<Product>()
            .CountAsync(cancellationToken);

        var totalOrders = await _context.Set<Order>()
            .CountAsync(cancellationToken);

        var totalBlogs = await _context.Set<Blog>()
            .CountAsync(cancellationToken);

        var totalReviews = await _context.Set<Review>()
            .CountAsync(cancellationToken);

        var totalRevenue = await _context.Set<Order>()
            .Where(o => o.Status == OrderStatus.Delivered)
            .SumAsync(o => o.FinalPrice, cancellationToken);

        var pendingOrders = await _context.Set<Order>()
            .CountAsync(o => o.Status == OrderStatus.Pending, cancellationToken);

        var completedOrders = await _context.Set<Order>()
            .CountAsync(o => o.Status == OrderStatus.Delivered, cancellationToken);

        var cancelledOrders = await _context.Set<Order>()
            .CountAsync(o => o.Status == OrderStatus.Cancelled, cancellationToken);

        var dto = new StatisticsDto
        {
            TotalUsers = totalUsers,
            TotalProducts = totalProducts,
            TotalOrders = totalOrders,
            TotalBlogs = totalBlogs,
            TotalReviews = totalReviews,
            TotalRevenue = totalRevenue,
            PendingOrders = pendingOrders,
            CompletedOrders = completedOrders,
            CancelledOrders = cancelledOrders
        };

        return ResponseViewModel<StatisticsDto>.Success(dto);
    }
}