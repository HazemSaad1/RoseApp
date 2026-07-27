using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Content;

namespace RoseApp.Application.Features.Blogs.Commands.DeleteBlog;

public class DeleteBlogCommandHandler
    : IRequestHandler<DeleteBlogCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public DeleteBlogCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        DeleteBlogCommand request,
        CancellationToken cancellationToken)
    {
        var blog = await _context.Set<Blog>()
            .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

        if (blog is null)
            throw new NotFoundException(nameof(Blog), request.Id);

        _context.Set<Blog>().Remove(blog);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Blog deleted successfully");
    }
}