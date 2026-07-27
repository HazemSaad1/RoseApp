namespace RoseApp.Application.DTOs.Admin;

public class AuditLogDto
{
    public Guid Id { get; set; }
    public string AdminName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public DateTime CreatedAt { get; set; }
}