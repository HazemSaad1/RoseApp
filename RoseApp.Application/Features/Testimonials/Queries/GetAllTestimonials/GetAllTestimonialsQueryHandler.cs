using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Testimonials;
using RoseApp.Domain.Entities.Content;

namespace RoseApp.Application.Features.Testimonials.Queries.GetAllTestimonials;

public class GetAllTestimonialsQueryHandler
    : IRequestHandler<GetAllTestimonialsQuery, ResponseViewModel<PaginationResult<TestimonialDto>>>
{
    private readonly IDbContext _context;

    public GetAllTestimonialsQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<PaginationResult<TestimonialDto>>> Handle(
        GetAllTestimonialsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Set<Testimonial>().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(t =>
                t.AuthorName.Contains(request.Search) ||
                t.Content.Contains(request.Search));

        var totalCount = await query.CountAsync(cancellationToken);

        var testimonials = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((request.Page - 1) * request.Limit)
            .Take(request.Limit)
            .Select(t => new TestimonialDto
            {
                Id = t.Id,
                AuthorName = t.AuthorName,
                AuthorPhoto = t.AuthorPhoto,
                Content = t.Content,
                Rating = t.Rating,
                CreatedAt = t.CreatedAt
            })
            .ToListAsync(cancellationToken);

        var result = PaginationResult<TestimonialDto>.Create(
            testimonials, totalCount, request.Page, request.Limit);

        return ResponseViewModel<PaginationResult<TestimonialDto>>.Success(result);
    }
}