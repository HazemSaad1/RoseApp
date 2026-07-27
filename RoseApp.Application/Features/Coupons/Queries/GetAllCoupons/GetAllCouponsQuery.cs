using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Coupons;

namespace RoseApp.Application.Features.Coupons.Queries.GetAllCoupons;

public record GetAllCouponsQuery(
    int Page = 1,
    int Limit = 20,
    string? Search = null)
    : IRequest<ResponseViewModel<PaginationResult<CouponDto>>>;