using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.SubCategories.Commands.CreateSubCategory;

public record CreateSubCategoryCommand(
    string Name,
    Guid CategoryId,
    string? PhotoUrl) : IRequest<ResponseViewModel<string>>;