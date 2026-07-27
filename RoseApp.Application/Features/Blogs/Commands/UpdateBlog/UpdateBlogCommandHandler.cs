using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Content;

namespace RoseApp.Application.Features.Blogs.Commands.UpdateBlog;

public class UpdateBlogCommandHandler
    : IRequestHandler<UpdateBlogCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public UpdateBlogCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        UpdateBlogCommand request,
        CancellationToken cancellationToken)
    {
        var blog = await _context.Set<Blog>()
            .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

        if (blog is null)
            throw new NotFoundException(nameof(Blog), request.Id);

        var newSlug = GenerateSlug(request.Title);

        var slugExists = await _context.Set<Blog>()
            .AnyAsync(b => b.Slug == newSlug && b.Id != request.Id, cancellationToken);

        if (slugExists)
            newSlug = $"{newSlug}-{Guid.NewGuid().ToString()[..8]}";

        blog.Title = request.Title;
        blog.Slug = newSlug;
        blog.Content = request.Content;

        if (request.PhotoUrl is not null)
            blog.PhotoUrl = request.PhotoUrl;

        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Blog updated successfully");
    }

    private static string GenerateSlug(string title)
        => title.ToLower().Trim().Replace(" ", "-").Replace("_", "-");
}