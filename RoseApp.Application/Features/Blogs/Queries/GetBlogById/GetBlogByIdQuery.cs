using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Blogs;

namespace RoseApp.Application.Features.Blogs.Queries.GetBlogById;

public record GetBlogByIdQuery(Guid Id)
    : IRequest<ResponseViewModel<BlogDto>>;