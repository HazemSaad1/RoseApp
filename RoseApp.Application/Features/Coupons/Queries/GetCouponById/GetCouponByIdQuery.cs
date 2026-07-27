using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Coupons;

namespace RoseApp.Application.Features.Coupons.Queries.GetCouponById;

public record GetCouponByIdQuery(Guid Id)
    : IRequest<ResponseViewModel<CouponDto>>;