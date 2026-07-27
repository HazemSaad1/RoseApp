using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Blogs;

namespace RoseApp.Application.Features.Blogs.Queries.GetAllBlogs;

public record GetAllBlogsQuery(
    int Page = 1,
    int Limit = 20,
    string? Search = null)
    : IRequest<ResponseViewModel<PaginationResult<BlogDto>>>;