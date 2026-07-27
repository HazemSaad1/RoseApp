using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Commerce;

namespace RoseApp.Application.Features.Coupons.Commands.UpdateCoupon;

public class UpdateCouponCommandHandler
    : IRequestHandler<UpdateCouponCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public UpdateCouponCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        UpdateCouponCommand request,
        CancellationToken cancellationToken)
    {
        var coupon = await _context.Set<Coupon>()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (coupon is null)
            throw new NotFoundException(nameof(Coupon), request.Id);

        var codeExists = await _context.Set<Coupon>()
            .AnyAsync(c => c.Code == request.Code
                && c.Id != request.Id, cancellationToken);

        if (codeExists)
            return ResponseViewModel<string>.Fail("Coupon code already exists", 409);

        coupon.Code = request.Code;
        coupon.DiscountType = request.DiscountType;
        coupon.DiscountValue = request.DiscountValue;
        coupon.MinOrderAmount = request.MinOrderAmount;
        coupon.MaxUses = request.MaxUses;
        coupon.ExpiresAt = request.ExpiresAt;
        coupon.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Coupon updated successfully");
    }
}