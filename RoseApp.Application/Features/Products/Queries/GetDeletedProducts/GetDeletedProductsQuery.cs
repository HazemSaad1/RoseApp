using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Products;

namespace RoseApp.Application.Features.Products.Queries.GetDeletedProducts;

public record GetDeletedProductsQuery(int Page = 1, int Limit = 20)
    : IRequest<ResponseViewModel<PaginationResult<ProductDto>>>;