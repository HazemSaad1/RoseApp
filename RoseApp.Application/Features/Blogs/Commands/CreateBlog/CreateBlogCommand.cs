using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Blogs.Commands.CreateBlog;

public record CreateBlogCommand(
    Guid AuthorId,
    string Title,
    string Content,
    string? PhotoUrl) : IRequest<ResponseViewModel<string>>;