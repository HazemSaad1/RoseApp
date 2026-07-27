namespace RoseApp.Application.DTOs.Testimonials;

public class TestimonialDto
{
    public Guid Id { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string? AuthorPhoto { get; set; }
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }
}