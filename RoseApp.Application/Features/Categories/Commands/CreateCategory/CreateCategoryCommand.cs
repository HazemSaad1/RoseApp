using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(
    string Name,
    string? PhotoUrl) : IRequest<ResponseViewModel<string>>;