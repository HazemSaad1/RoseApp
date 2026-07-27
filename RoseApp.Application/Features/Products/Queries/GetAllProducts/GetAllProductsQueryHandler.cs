using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Products;
using RoseApp.Domain.Entities.Catalog;

namespace RoseApp.Application.Features.Products.Queries.GetAllProducts;

public class GetAllProductsQueryHandler
    : IRequestHandler<GetAllProductsQuery, ResponseViewModel<PaginationResult<ProductDto>>>
{
    private readonly IDbContext _context;

    public GetAllProductsQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<PaginationResult<ProductDto>>> Handle(
        GetAllProductsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Set<Product>()
            .Include(p => p.Category)
            .Include(p => p.SubCategories)
            .Include(p => p.Occasions)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Filter.Search))
            query = query.Where(p =>
                p.Name.Contains(request.Filter.Search) ||
                p.Description.Contains(request.Filter.Search));

        if (request.Filter.CategoryId.HasValue)
            query = query.Where(p => p.CategoryId == request.Filter.CategoryId.Value);

        if (request.Filter.SubCategoryId.HasValue)
            query = query.Where(p =>
                p.SubCategories.Any(s => s.Id == request.Filter.SubCategoryId.Value));

        if (request.Filter.OccasionId.HasValue)
            query = query.Where(p =>
                p.Occasions.Any(o => o.Id == request.Filter.OccasionId.Value));

        if (request.Filter.MinPrice.HasValue)
            query = query.Where(p => p.Price >= request.Filter.MinPrice.Value);

        if (request.Filter.MaxPrice.HasValue)
            query = query.Where(p => p.Price <= request.Filter.MaxPrice.Value);

        query = request.Filter.SortBy?.ToLower() switch
        {
            "price" => request.Filter.SortOrder == "desc"
                ? query.OrderByDescending(p => p.Price)
                : query.OrderBy(p => p.Price),
            "name" => request.Filter.SortOrder == "desc"
                ? query.OrderByDescending(p => p.Name)
                : query.OrderBy(p => p.Name),
            _ => query.OrderByDescending(p => p.CreatedAt)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var products = await query
            .Skip((request.Filter.Page - 1) * request.Filter.Limit)
            .Take(request.Filter.Limit)
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
            products, totalCount, request.Filter.Page, request.Filter.Limit);

        return ResponseViewModel<PaginationResult<ProductDto>>.Success(result);
    }
}