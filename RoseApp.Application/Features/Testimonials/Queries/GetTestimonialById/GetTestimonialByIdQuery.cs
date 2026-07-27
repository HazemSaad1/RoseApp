using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Testimonials;

namespace RoseApp.Application.Features.Testimonials.Queries.GetTestimonialById;

public record GetTestimonialByIdQuery(Guid Id)
    : IRequest<ResponseViewModel<TestimonialDto>>;