using RoseApp.Domain.Abstractions;
using RoseApp.Domain.Entities.Identity;

namespace RoseApp.Domain.Entities.UserDomain
{
    public class Address : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public string Label { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string? ZipCode { get; set; }
        public bool IsDefault { get; set; }
    }
}
