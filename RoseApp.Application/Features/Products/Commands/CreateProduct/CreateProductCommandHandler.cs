using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Catalog;

namespace RoseApp.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler
    : IRequestHandler<CreateProductCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public CreateProductCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var category = await _context.Set<Category>()
            .FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);

        if (category is null)
            throw new NotFoundException(nameof(Category), request.CategoryId);

        var exists = await _context.Set<Product>()
            .AnyAsync(p => p.Name.ToLower() == request.Name.ToLower(), cancellationToken);

        if (exists)
            return ResponseViewModel<string>.Fail("Product already exists", 409);

        var subCategories = await _context.Set<SubCategory>()
            .Where(s => request.SubCategoryIds.Contains(s.Id))
            .ToListAsync(cancellationToken);

        var occasions = await _context.Set<Occasion>()
            .Where(o => request.OccasionIds.Contains(o.Id))
            .ToListAsync(cancellationToken);

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Slug = GenerateSlug(request.Name),
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            PhotoUrl = request.PhotoUrl,
            CategoryId = request.CategoryId,
            SubCategories = subCategories,
            Occasions = occasions
        };

        await _context.Set<Product>().AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success(
            "Product created successfully", "Created", 201);
    }

    private static string GenerateSlug(string name)
        => name.ToLower().Trim().Replace(" ", "-").Replace("_", "-");
}