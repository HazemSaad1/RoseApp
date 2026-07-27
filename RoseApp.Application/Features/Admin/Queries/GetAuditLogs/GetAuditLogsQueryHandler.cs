using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Admin;
using RoseApp.Domain.Entities.Admin;

namespace RoseApp.Application.Features.Admin.Queries.GetAuditLogs;

public class GetAuditLogsQueryHandler
    : IRequestHandler<GetAuditLogsQuery, ResponseViewModel<PaginationResult<AuditLogDto>>>
{
    private readonly IDbContext _context;

    public GetAuditLogsQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<PaginationResult<AuditLogDto>>> Handle(
        GetAuditLogsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Set<AdminAuditLog>()
            .Include(a => a.Admin)
            .AsNoTracking()
            .OrderByDescending(a => a.CreatedAt);

        var totalCount = await query.CountAsync(cancellationToken);

        var logs = await query
            .Skip((request.Page - 1) * request.Limit)
            .Take(request.Limit)
            .Select(a => new AuditLogDto
            {
                Id = a.Id,
                AdminName = a.Admin.UserName,
                Action = a.Action,
                EntityName = a.EntityName,
                EntityId = a.EntityId,
                OldValues = a.OldValues,
                NewValues = a.NewValues,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync(cancellationToken);

        var result = PaginationResult<AuditLogDto>.Create(
            logs, totalCount, request.Page, request.Limit);

        return ResponseViewModel<PaginationResult<AuditLogDto>>.Success(result);
    }
}