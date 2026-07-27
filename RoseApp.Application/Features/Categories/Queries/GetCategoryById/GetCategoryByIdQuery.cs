using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Categories;

namespace RoseApp.Application.Features.Categories.Queries.GetCategoryById;

public record GetCategoryByIdQuery(Guid Id)
    : IRequest<ResponseViewModel<CategoryDto>>;