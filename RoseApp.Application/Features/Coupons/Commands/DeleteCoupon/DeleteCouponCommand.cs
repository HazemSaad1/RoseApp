using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Coupons.Commands.DeleteCoupon;

public record DeleteCouponCommand(Guid Id)
    : IRequest<ResponseViewModel<string>>;