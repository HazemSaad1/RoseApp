namespace RoseApp.Application.DTOs.Occasions;

public class OccasionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; }
}