using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Testimonials.Commands.CreateTestimonial;

public record CreateTestimonialCommand(
    string AuthorName,
    string? AuthorPhoto,
    string Content,
    int Rating) : IRequest<ResponseViewModel<string>>;