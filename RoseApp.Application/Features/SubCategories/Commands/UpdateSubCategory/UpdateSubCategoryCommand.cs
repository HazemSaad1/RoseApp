using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.SubCategories.Commands.UpdateSubCategory;

public record UpdateSubCategoryCommand(
    Guid Id,
    string Name,
    Guid CategoryId,
    string? PhotoUrl) : IRequest<ResponseViewModel<string>>;