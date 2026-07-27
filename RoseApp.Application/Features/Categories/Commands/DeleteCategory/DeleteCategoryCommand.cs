using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand(Guid Id)
    : IRequest<ResponseViewModel<string>>;