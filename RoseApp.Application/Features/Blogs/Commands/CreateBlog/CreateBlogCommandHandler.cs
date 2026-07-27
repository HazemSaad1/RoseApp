using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Domain.Entities.Content;

namespace RoseApp.Application.Features.Blogs.Commands.CreateBlog;

public class CreateBlogCommandHandler
    : IRequestHandler<CreateBlogCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public CreateBlogCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        CreateBlogCommand request,
        CancellationToken cancellationToken)
    {
        var slug = GenerateSlug(request.Title);

        var slugExists = await _context.Set<Blog>()
            .AnyAsync(b => b.Slug == slug, cancellationToken);

        if (slugExists)
            slug = $"{slug}-{Guid.NewGuid().ToString()[..8]}";

        var blog = new Blog
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Slug = slug,
            Content = request.Content,
            PhotoUrl = request.PhotoUrl,
            AuthorId = request.AuthorId
        };

        await _context.Set<Blog>().AddAsync(blog, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success(
            "Blog created successfully", "Created", 201);
    }

    private static string GenerateSlug(string title)
        => title.ToLower().Trim().Replace(" ", "-").Replace("_", "-");
}