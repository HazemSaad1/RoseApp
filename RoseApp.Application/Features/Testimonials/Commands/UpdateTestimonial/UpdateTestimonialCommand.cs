using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Testimonials.Commands.UpdateTestimonial;

public record UpdateTestimonialCommand(
    Guid Id,
    string AuthorName,
    string? AuthorPhoto,
    string Content,
    int Rating) : IRequest<ResponseViewModel<string>>;