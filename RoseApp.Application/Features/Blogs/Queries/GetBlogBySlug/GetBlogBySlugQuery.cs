using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Blogs;

namespace RoseApp.Application.Features.Blogs.Queries.GetBlogBySlug;

public record GetBlogBySlugQuery(string Slug)
    : IRequest<ResponseViewModel<BlogDto>>;