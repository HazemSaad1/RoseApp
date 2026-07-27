using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Admin;

namespace RoseApp.Application.Features.Admin.Queries.GetStatistics;

public record GetStatisticsQuery()
    : IRequest<ResponseViewModel<StatisticsDto>>;