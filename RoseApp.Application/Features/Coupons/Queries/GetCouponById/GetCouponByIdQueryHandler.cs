using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Coupons;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Commerce;

namespace RoseApp.Application.Features.Coupons.Queries.GetCouponById;

public class GetCouponByIdQueryHandler
    : IRequestHandler<GetCouponByIdQuery, ResponseViewModel<CouponDto>>
{
    private readonly IDbContext _context;

    public GetCouponByIdQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<CouponDto>> Handle(
        GetCouponByIdQuery request,
        CancellationToken cancellationToken)
    {
        var coupon = await _context.Set<Coupon>()
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (coupon is null)
            throw new NotFoundException(nameof(Coupon), request.Id);

        return ResponseViewModel<CouponDto>.Success(new CouponDto
        {
            Id = coupon.Id,
            Code = coupon.Code,
            DiscountType = coupon.DiscountType.ToString(),
            DiscountValue = coupon.DiscountValue,
            MinOrderAmount = coupon.MinOrderAmount,
            MaxUses = coupon.MaxUses,
            UsedCount = coupon.UsedCount,
            ExpiresAt = coupon.ExpiresAt,
            IsActive = coupon.IsActive
        });
    }
}