using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.SubCategories.Commands.DeleteSubCategory;

public record DeleteSubCategoryCommand(Guid Id)
    : IRequest<ResponseViewModel<string>>;