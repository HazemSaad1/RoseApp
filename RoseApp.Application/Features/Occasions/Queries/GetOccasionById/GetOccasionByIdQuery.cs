using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Occasions;

namespace RoseApp.Application.Features.Occasions.Queries.GetOccasionById;

public record GetOccasionByIdQuery(Guid Id)
    : IRequest<ResponseViewModel<OccasionDto>>;