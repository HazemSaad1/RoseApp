using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Admin;

namespace RoseApp.Application.Features.Admin.Queries.GetAuditLogs;

public record GetAuditLogsQuery(int Page = 1, int Limit = 20)
    : IRequest<ResponseViewModel<PaginationResult<AuditLogDto>>>;