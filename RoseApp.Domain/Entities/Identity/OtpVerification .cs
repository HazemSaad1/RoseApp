using RoseApp.Domain.Abstractions;
using RoseApp.Domain.Enums;

namespace RoseApp.Domain.Entities.Identity
{
    public class OtpVerification:BaseEntity<Guid>
    {
        public string Email { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public OtpType Type { get; set; }
        public bool IsUsed { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
