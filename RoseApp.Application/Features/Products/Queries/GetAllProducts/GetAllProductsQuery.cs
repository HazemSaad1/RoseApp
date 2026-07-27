using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Products;

namespace RoseApp.Application.Features.Products.Queries.GetAllProducts;

public record GetAllProductsQuery(ProductFilterDto Filter)
    : IRequest<ResponseViewModel<PaginationResult<ProductDto>>>;