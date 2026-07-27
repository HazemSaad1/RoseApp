using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Catalog;

namespace RoseApp.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler
    : IRequestHandler<UpdateProductCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public UpdateProductCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _context.Set<Product>()
            .Include(p => p.SubCategories)
            .Include(p => p.Occasions)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (product is null)
            throw new NotFoundException(nameof(Product), request.Id);

        var categoryExists = await _context.Set<Category>()
            .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);

        if (!categoryExists)
            throw new NotFoundException(nameof(Category), request.CategoryId);

        var nameExists = await _context.Set<Product>()
            .AnyAsync(p => p.Name.ToLower() == request.Name.ToLower()
                && p.Id != request.Id, cancellationToken);

        if (nameExists)
            return ResponseViewModel<string>.Fail("Product name already exists", 409);

        var subCategories = await _context.Set<SubCategory>()
            .Where(s => request.SubCategoryIds.Contains(s.Id))
            .ToListAsync(cancellationToken);

        var occasions = await _context.Set<Occasion>()
            .Where(o => request.OccasionIds.Contains(o.Id))
            .ToListAsync(cancellationToken);

        product.Name = request.Name;
        product.Slug = GenerateSlug(request.Name);
        product.Description = request.Description;
        product.Price = request.Price;
        product.Stock = request.Stock;
        product.CategoryId = request.CategoryId;
        product.SubCategories = subCategories;
        product.Occasions = occasions;

        if (request.PhotoUrl is not null)
            product.PhotoUrl = request.PhotoUrl;

        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Product updated successfully");
    }

    private static string GenerateSlug(string name)
        => name.ToLower().Trim().Replace(" ", "-").Replace("_", "-");
}