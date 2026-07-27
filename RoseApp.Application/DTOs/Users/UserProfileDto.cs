namespace RoseApp.Application.DTOs.Users
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? PhotoUrl { get; set; }
        public List<string> Roles { get; set; } = [];
    }
}
