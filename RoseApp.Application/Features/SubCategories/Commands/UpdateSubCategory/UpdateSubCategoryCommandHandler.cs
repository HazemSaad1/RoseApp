using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Catalog;

namespace RoseApp.Application.Features.SubCategories.Commands.UpdateSubCategory;

public class UpdateSubCategoryCommandHandler
    : IRequestHandler<UpdateSubCategoryCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public UpdateSubCategoryCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        UpdateSubCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var subCategory = await _context.Set<SubCategory>()
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (subCategory is null)
            throw new NotFoundException(nameof(SubCategory), request.Id);

        var categoryExists = await _context.Set<Category>()
            .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);

        if (!categoryExists)
            throw new NotFoundException(nameof(Category), request.CategoryId);

        var nameExists = await _context.Set<SubCategory>()
            .AnyAsync(s => s.Name.ToLower() == request.Name.ToLower()
                && s.CategoryId == request.CategoryId
                && s.Id != request.Id, cancellationToken);

        if (nameExists)
            return ResponseViewModel<string>.Fail(
                "SubCategory name already exists in this category", 409);

        subCategory.Name = request.Name;
        subCategory.Slug = GenerateSlug(request.Name);
        subCategory.CategoryId = request.CategoryId;

        if (request.PhotoUrl is not null)
            subCategory.PhotoUrl = request.PhotoUrl;

        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("SubCategory updated successfully");
    }

    private static string GenerateSlug(string name)
        => name.ToLower().Trim().Replace(" ", "-").Replace("_", "-");
}