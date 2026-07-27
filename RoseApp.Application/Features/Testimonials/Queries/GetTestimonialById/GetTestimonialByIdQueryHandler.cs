using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Testimonials;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Content;

namespace RoseApp.Application.Features.Testimonials.Queries.GetTestimonialById;

public class GetTestimonialByIdQueryHandler
    : IRequestHandler<GetTestimonialByIdQuery, ResponseViewModel<TestimonialDto>>
{
    private readonly IDbContext _context;

    public GetTestimonialByIdQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<TestimonialDto>> Handle(
        GetTestimonialByIdQuery request,
        CancellationToken cancellationToken)
    {
        var testimonial = await _context.Set<Testimonial>()
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (testimonial is null)
            throw new NotFoundException(nameof(Testimonial), request.Id);

        return ResponseViewModel<TestimonialDto>.Success(new TestimonialDto
        {
            Id = testimonial.Id,
            AuthorName = testimonial.AuthorName,
            AuthorPhoto = testimonial.AuthorPhoto,
            Content = testimonial.Content,
            Rating = testimonial.Rating,
            CreatedAt = testimonial.CreatedAt
        });
    }
}