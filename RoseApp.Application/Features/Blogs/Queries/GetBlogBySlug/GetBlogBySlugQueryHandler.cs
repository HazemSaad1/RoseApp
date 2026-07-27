using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Blogs;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Content;

namespace RoseApp.Application.Features.Blogs.Queries.GetBlogBySlug;

public class GetBlogBySlugQueryHandler
    : IRequestHandler<GetBlogBySlugQuery, ResponseViewModel<BlogDto>>
{
    private readonly IDbContext _context;

    public GetBlogBySlugQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<BlogDto>> Handle(
        GetBlogBySlugQuery request,
        CancellationToken cancellationToken)
    {
        var blog = await _context.Set<Blog>()
            .Include(b => b.Author)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Slug == request.Slug, cancellationToken);

        if (blog is null)
            throw new NotFoundException(nameof(Blog), request.Slug);

        return ResponseViewModel<BlogDto>.Success(new BlogDto
        {
            Id = blog.Id,
            Title = blog.Title,
            Slug = blog.Slug,
            Content = blog.Content,
            PhotoUrl = blog.PhotoUrl,
            AuthorName = blog.Author.UserName,
            CreatedAt = blog.CreatedAt,
            UpdatedAt = blog.UpdatedAt
        });
    }
}