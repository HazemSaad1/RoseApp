using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Blogs.Commands.UpdateBlog;

public record UpdateBlogCommand(
    Guid Id,
    string Title,
    string Content,
    string? PhotoUrl) : IRequest<ResponseViewModel<string>>;