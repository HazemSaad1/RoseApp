using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.SubCategories;

namespace RoseApp.Application.Features.SubCategories.Queries.GetAllSubCategories;

public record GetAllSubCategoriesQuery(
    int Page = 1,
    int Limit = 20,
    string? Search = null)
    : IRequest<ResponseViewModel<PaginationResult<SubCategoryDto>>>;