using RoseApp.Application.Contracts;
using RoseApp.Application.Contracts.IServices;
using RoseApp.Domain.Entities.Admin;

namespace RoseApp.Infrastructure.Services;

public class AuditLogService : IAuditLogService
{
    private readonly IDbContext _context;

    public AuditLogService(IDbContext context)
    {
        _context = context;
    }

    public async Task LogAsync(
        Guid adminId,
        string action,
        string entityName,
        string entityId,
        string? oldValues = null,
        string? newValues = null)
    {
        var log = new AdminAuditLog
        {
            Id = Guid.NewGuid(),
            AdminId = adminId,
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            OldValues = oldValues,
            NewValues = newValues
        };

        await _context.Set<AdminAuditLog>().AddAsync(log);
        await _context.SaveChangesAsync();
    }
}