using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Testimonials.Commands.DeleteTestimonial;

public record DeleteTestimonialCommand(Guid Id)
    : IRequest<ResponseViewModel<string>>;