using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Blogs.Commands.DeleteBlog;

public record DeleteBlogCommand(Guid Id)
    : IRequest<ResponseViewModel<string>>;