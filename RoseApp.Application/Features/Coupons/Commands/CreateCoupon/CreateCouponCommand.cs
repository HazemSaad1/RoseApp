using MediatR;
using RoseApp.Application.Common;
using RoseApp.Domain.Enums;

namespace RoseApp.Application.Features.Coupons.Commands.CreateCoupon;

public record CreateCouponCommand(
    string Code,
    DiscountType DiscountType,
    decimal DiscountValue,
    decimal? MinOrderAmount,
    int? MaxUses,
    DateTime? ExpiresAt) : IRequest<ResponseViewModel<string>>;