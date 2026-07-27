using MediatR;
using RoseApp.Application.Common;
using RoseApp.Domain.Enums;

namespace RoseApp.Application.Features.Coupons.Commands.UpdateCoupon;

public record UpdateCouponCommand(
    Guid Id,
    string Code,
    DiscountType DiscountType,
    decimal DiscountValue,
    decimal? MinOrderAmount,
    int? MaxUses,
    DateTime? ExpiresAt,
    bool IsActive) : IRequest<ResponseViewModel<string>>;