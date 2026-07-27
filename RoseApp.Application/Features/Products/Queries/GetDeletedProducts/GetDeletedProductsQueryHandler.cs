using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Products;
using RoseApp.Domain.Entities.Catalog;

namespace RoseApp.Application.Features.Products.Queries.GetDeletedProducts;

public class GetDeletedProductsQueryHandler
    : IRequestHandler<GetDeletedProductsQuery, ResponseViewModel<PaginationResult<ProductDto>>>
{
    private readonly IDbContext _context;

    public GetDeletedProductsQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<PaginationResult<ProductDto>>> Handle(
        GetDeletedProductsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Set<Product>()
            .IgnoreQueryFilters()
            .Include(p => p.Category)
            .Include(p => p.SubCategories)
            .Include(p => p.Occasions)
            .AsNoTracking()
            .Where(p => p.IsDeleted);

        var totalCount = await query.CountAsync(cancellationToken);

        var products = await query
            .OrderByDescending(p => p.DeletedAt)
            .Skip((request.Page - 1) * request.Limit)
            .Take(request.Limit)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Slug = p.Slug,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                PhotoUrl = p.PhotoUrl,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                SubCategories = p.SubCategories.Select(s => s.Name).ToList(),
                Occasions = p.Occasions.Select(o => o.Name).ToList()
            })
            .ToListAsync(cancellationToken);

        var result = PaginationResult<ProductDto>.Create(
            products, totalCount, request.Page, request.Limit);

        return ResponseViewModel<PaginationResult<ProductDto>>.Success(result);
    }
}