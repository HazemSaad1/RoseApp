using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand(
    Guid Id,
    string Name,
    string? PhotoUrl) : IRequest<ResponseViewModel<string>>;