using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Catalog;

namespace RoseApp.Application.Features.SubCategories.Commands.CreateSubCategory;

public class CreateSubCategoryCommandHandler
    : IRequestHandler<CreateSubCategoryCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public CreateSubCategoryCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        CreateSubCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var categoryExists = await _context.Set<Category>()
            .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);

        if (!categoryExists)
            throw new NotFoundException(nameof(Category), request.CategoryId);

        var exists = await _context.Set<SubCategory>()
            .AnyAsync(s => s.Name.ToLower() == request.Name.ToLower()
                && s.CategoryId == request.CategoryId, cancellationToken);

        if (exists)
            return ResponseViewModel<string>.Fail(
                "SubCategory already exists in this category", 409);

        var subCategory = new SubCategory
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Slug = GenerateSlug(request.Name),
            PhotoUrl = request.PhotoUrl,
            CategoryId = request.CategoryId
        };

        await _context.Set<SubCategory>().AddAsync(subCategory, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success(
            "SubCategory created successfully", "Created", 201);
    }

    private static string GenerateSlug(string name)
        => name.ToLower().Trim().Replace(" ", "-").Replace("_", "-");
}