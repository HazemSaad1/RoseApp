using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Reviews;

namespace RoseApp.Application.Features.Reviews.Queries.GetReviews;

public record GetReviewsQuery(
    int Page = 1,
    int Limit = 20,
    Guid? ProductId = null,
    Guid? UserId = null,
    string? Search = null)
    : IRequest<ResponseViewModel<PaginationResult<ReviewDto>>>;