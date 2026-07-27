using System;
using System.Collections.Generic;
using System.Text;

namespace RoseApp.Application.Contracts
{
    public interface IAuditLogService
    {
        Task LogAsync(
            Guid adminId,
            string action,
            string entityName,
            string entityId,
            string? oldValues = null,
            string? newValues = null);
    }
}
