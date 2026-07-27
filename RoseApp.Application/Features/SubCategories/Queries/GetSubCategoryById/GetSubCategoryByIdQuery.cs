using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.SubCategories;

namespace RoseApp.Application.Features.SubCategories.Queries.GetSubCategoryById;

public record GetSubCategoryByIdQuery(Guid Id)
    : IRequest<ResponseViewModel<SubCategoryDto>>;