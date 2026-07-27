using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Domain.Entities.Commerce;

namespace RoseApp.Application.Features.Coupons.Commands.CreateCoupon;

public class CreateCouponCommandHandler
    : IRequestHandler<CreateCouponCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public CreateCouponCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        CreateCouponCommand request,
        CancellationToken cancellationToken)
    {
        var exists = await _context.Set<Coupon>()
            .AnyAsync(c => c.Code == request.Code, cancellationToken);

        if (exists)
            return ResponseViewModel<string>.Fail("Coupon code already exists", 409);

        var coupon = new Coupon
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            DiscountType = request.DiscountType,
            DiscountValue = request.DiscountValue,
            MinOrderAmount = request.MinOrderAmount,
            MaxUses = request.MaxUses,
            UsedCount = 0,
            ExpiresAt = request.ExpiresAt,
            IsActive = true
        };

        await _context.Set<Coupon>().AddAsync(coupon, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success(
            "Coupon created successfully", "Created", 201);
    }
}