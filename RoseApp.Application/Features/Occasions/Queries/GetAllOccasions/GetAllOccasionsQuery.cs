using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Occasions;

namespace RoseApp.Application.Features.Occasions.Queries.GetAllOccasions;

public record GetAllOccasionsQuery(
    int Page = 1,
    int Limit = 20,
    string? Search = null)
    : IRequest<ResponseViewModel<PaginationResult<OccasionDto>>>;