using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Categories;

namespace RoseApp.Application.Features.Categories.Queries.GetAllCategories;

public record GetAllCategoriesQuery(
    int Page = 1,
    int Limit = 20,
    string? Search = null)
    : IRequest<ResponseViewModel<PaginationResult<CategoryDto>>>;