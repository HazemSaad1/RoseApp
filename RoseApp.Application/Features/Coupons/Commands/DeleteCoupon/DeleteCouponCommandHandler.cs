using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Commerce;

namespace RoseApp.Application.Features.Coupons.Commands.DeleteCoupon;

public class DeleteCouponCommandHandler
    : IRequestHandler<DeleteCouponCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public DeleteCouponCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        DeleteCouponCommand request,
        CancellationToken cancellationToken)
    {
        var coupon = await _context.Set<Coupon>()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (coupon is null)
            throw new NotFoundException(nameof(Coupon), request.Id);

        _context.Set<Coupon>().Remove(coupon);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Coupon deleted successfully");
    }
}