using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Testimonials;

namespace RoseApp.Application.Features.Testimonials.Queries.GetAllTestimonials;

public record GetAllTestimonialsQuery(
    int Page = 1,
    int Limit = 20,
    string? Search = null)
    : IRequest<ResponseViewModel<PaginationResult<TestimonialDto>>>;