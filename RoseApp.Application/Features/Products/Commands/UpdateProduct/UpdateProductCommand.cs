using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int Stock,
    string? PhotoUrl,
    Guid CategoryId,
    List<Guid> SubCategoryIds,
    List<Guid> OccasionIds) : IRequest<ResponseViewModel<string>>;