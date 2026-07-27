using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Products;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Catalog;

namespace RoseApp.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler
    : IRequestHandler<GetProductByIdQuery, ResponseViewModel<ProductDto>>
{
    private readonly IDbContext _context;

    public GetProductByIdQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<ProductDto>> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var product = await _context.Set<Product>()
            .Include(p => p.Category)
            .Include(p => p.SubCategories)
            .Include(p => p.Occasions)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (product is null)
            throw new NotFoundException(nameof(Product), request.Id);

        return ResponseViewModel<ProductDto>.Success(new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Slug = product.Slug,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            PhotoUrl = product.PhotoUrl,
            CategoryId = product.CategoryId,
            CategoryName = product.Category.Name,
            SubCategories = product.SubCategories.Select(s => s.Name).ToList(),
            Occasions = product.Occasions.Select(o => o.Name).ToList()
        });
    }
}