using RoseApp.Domain.Abstractions;
using RoseApp.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoseApp.Domain.Entities.Admin
{
    public class AdminAuditLog : BaseEntity<Guid>
    {
        public Guid AdminId { get; set; }
        public User Admin { get; set; } = null!;
        public string Action { get; set; } = string.Empty;
        public string EntityName { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
    }
}
