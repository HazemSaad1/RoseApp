using RoseApp.Domain.Abstractions;

namespace RoseApp.Domain.Entities.Identity
{
    public class User:BaseEntity<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? PhotoUrl { get; set; }
        public bool IsEmailVerified { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = [];
    }
}
