using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Blogs;
using RoseApp.Domain.Entities.Content;

namespace RoseApp.Application.Features.Blogs.Queries.GetAllBlogs;

public class GetAllBlogsQueryHandler
    : IRequestHandler<GetAllBlogsQuery, ResponseViewModel<PaginationResult<BlogDto>>>
{
    private readonly IDbContext _context;

    public GetAllBlogsQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<PaginationResult<BlogDto>>> Handle(
        GetAllBlogsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Set<Blog>()
            .Include(b => b.Author)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(b =>
                b.Title.Contains(request.Search) ||
                b.Slug.Contains(request.Search) ||
                b.Content.Contains(request.Search));

        var totalCount = await query.CountAsync(cancellationToken);

        var blogs = await query
            .OrderByDescending(b => b.CreatedAt)
            .Skip((request.Page - 1) * request.Limit)
            .Take(request.Limit)
            .Select(b => new BlogDto
            {
                Id = b.Id,
                Title = b.Title,
                Slug = b.Slug,
                Content = b.Content,
                PhotoUrl = b.PhotoUrl,
                AuthorName = b.Author.UserName,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        var result = PaginationResult<BlogDto>.Create(
            blogs, totalCount, request.Page, request.Limit);

        return ResponseViewModel<PaginationResult<BlogDto>>.Success(result);
    }
}