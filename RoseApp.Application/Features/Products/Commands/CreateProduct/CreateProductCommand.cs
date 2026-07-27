using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    string? PhotoUrl,
    Guid CategoryId,
    List<Guid> SubCategoryIds,
    List<Guid> OccasionIds) : IRequest<ResponseViewModel<string>>;