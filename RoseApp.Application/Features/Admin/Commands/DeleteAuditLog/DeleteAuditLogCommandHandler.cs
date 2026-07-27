using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Admin;

namespace RoseApp.Application.Features.Admin.Commands.DeleteAuditLog;

public class DeleteAuditLogCommandHandler
    : IRequestHandler<DeleteAuditLogCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public DeleteAuditLogCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        DeleteAuditLogCommand request,
        CancellationToken cancellationToken)
    {
        var log = await _context.Set<AdminAuditLog>()
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (log is null)
            throw new NotFoundException(nameof(AdminAuditLog), request.Id);

        _context.Set<AdminAuditLog>().Remove(log);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Audit log deleted successfully");
    }
}