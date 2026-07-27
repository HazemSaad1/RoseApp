using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Domain.Entities.Catalog;

namespace RoseApp.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler
    : IRequestHandler<CreateCategoryCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public CreateCategoryCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var exists = await _context.Set<Category>()
            .AnyAsync(c => c.Name.ToLower() == request.Name.ToLower(), cancellationToken);

        if (exists)
            return ResponseViewModel<string>.Fail("Category already exists", 409);

        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Slug = GenerateSlug(request.Name),
            PhotoUrl = request.PhotoUrl
        };

        await _context.Set<Category>().AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success(
            "Category created successfully", "Created", 201);
    }

    private static string GenerateSlug(string name)
        => name.ToLower().Trim().Replace(" ", "-").Replace("_", "-");
}