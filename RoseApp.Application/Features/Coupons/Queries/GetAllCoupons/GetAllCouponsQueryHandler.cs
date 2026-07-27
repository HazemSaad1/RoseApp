using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Coupons;
using RoseApp.Domain.Entities.Commerce;

namespace RoseApp.Application.Features.Coupons.Queries.GetAllCoupons;

public class GetAllCouponsQueryHandler
    : IRequestHandler<GetAllCouponsQuery, ResponseViewModel<PaginationResult<CouponDto>>>
{
    private readonly IDbContext _context;

    public GetAllCouponsQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<PaginationResult<CouponDto>>> Handle(
        GetAllCouponsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Set<Coupon>().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(c =>
                c.Code.Contains(request.Search));

        var totalCount = await query.CountAsync(cancellationToken);

        var coupons = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((request.Page - 1) * request.Limit)
            .Take(request.Limit)
            .Select(c => new CouponDto
            {
                Id = c.Id,
                Code = c.Code,
                DiscountType = c.DiscountType.ToString(),
                DiscountValue = c.DiscountValue,
                MinOrderAmount = c.MinOrderAmount,
                MaxUses = c.MaxUses,
                UsedCount = c.UsedCount,
                ExpiresAt = c.ExpiresAt,
                IsActive = c.IsActive
            })
            .ToListAsync(cancellationToken);

        var result = PaginationResult<CouponDto>.Create(
            coupons, totalCount, request.Page, request.Limit);

        return ResponseViewModel<PaginationResult<CouponDto>>.Success(result);
    }
}   