using RoseApp.Domain.Abstractions;

namespace RoseApp.Domain.Entities.Identity
{
    public class Role:BaseEntity<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = [];
        public ICollection<RolePermission> RolePermissions { get; set; } = [];
    }
}
