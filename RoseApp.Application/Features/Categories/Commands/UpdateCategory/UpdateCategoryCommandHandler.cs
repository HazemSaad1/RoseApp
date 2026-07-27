using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Catalog;

namespace RoseApp.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler
    : IRequestHandler<UpdateCategoryCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public UpdateCategoryCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        UpdateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var category = await _context.Set<Category>()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (category is null)
            throw new NotFoundException(nameof(Category), request.Id);

        var nameExists = await _context.Set<Category>()
            .AnyAsync(c => c.Name.ToLower() == request.Name.ToLower()
                && c.Id != request.Id, cancellationToken);

        if (nameExists)
            return ResponseViewModel<string>.Fail("Category name already exists", 409);

        category.Name = request.Name;
        category.Slug = GenerateSlug(request.Name);

        if (request.PhotoUrl is not null)
            category.PhotoUrl = request.PhotoUrl;

        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Category updated successfully");
    }

    private static string GenerateSlug(string name)
        => name.ToLower().Trim().Replace(" ", "-").Replace("_", "-");
}