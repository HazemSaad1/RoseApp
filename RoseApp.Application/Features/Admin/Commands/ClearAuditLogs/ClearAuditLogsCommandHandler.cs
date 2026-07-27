using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Domain.Entities.Admin;

namespace RoseApp.Application.Features.Admin.Commands.ClearAuditLogs;

public class ClearAuditLogsCommandHandler
    : IRequestHandler<ClearAuditLogsCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public ClearAuditLogsCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        ClearAuditLogsCommand request,
        CancellationToken cancellationToken)
    {
        var logs = await _context.Set<AdminAuditLog>()
            .ToListAsync(cancellationToken);

        if (!logs.Any())
            return ResponseViewModel<string>.Success("No audit logs to clear");

        _context.Set<AdminAuditLog>().RemoveRange(logs);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("All audit logs cleared successfully");
    }
}