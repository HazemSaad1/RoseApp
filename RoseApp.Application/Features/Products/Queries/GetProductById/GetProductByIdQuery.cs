using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Products;

namespace RoseApp.Application.Features.Products.Queries.GetProductById;

public record GetProductByIdQuery(Guid Id)
    : IRequest<ResponseViewModel<ProductDto>>;