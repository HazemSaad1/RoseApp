using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Reviews;
using RoseApp.Domain.Entities.UserDomain;

namespace RoseApp.Application.Features.Reviews.Queries.GetReviews;

public class GetReviewsQueryHandler
    : IRequestHandler<GetReviewsQuery, ResponseViewModel<PaginationResult<ReviewDto>>>
{
    private readonly IDbContext _context;

    public GetReviewsQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<PaginationResult<ReviewDto>>> Handle(
        GetReviewsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Set<Review>()
            .Include(r => r.User)
            .AsNoTracking()
            .AsQueryable();

        if (request.ProductId.HasValue)
            query = query.Where(r => r.ProductId == request.ProductId.Value);

        if (request.UserId.HasValue)
            query = query.Where(r => r.UserId == request.UserId.Value);

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(r =>
                r.Comment != null &&
                r.Comment.Contains(request.Search));

        var totalCount = await query.CountAsync(cancellationToken);

        var reviews = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((request.Page - 1) * request.Limit)
            .Take(request.Limit)
            .Select(r => new ReviewDto
            {
                Id = r.Id,
                UserId = r.UserId,
                UserName = r.User.UserName,
                UserPhoto = r.User.PhotoUrl ?? string.Empty,
                ProductId = r.ProductId,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync(cancellationToken);

        var result = PaginationResult<ReviewDto>.Create(
            reviews, totalCount, request.Page, request.Limit);

        return ResponseViewModel<PaginationResult<ReviewDto>>.Success(result);
    }
}