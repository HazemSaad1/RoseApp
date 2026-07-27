// Features/Categories/Queries/GetCategoryById/GetCategoryByIdQueryHandler.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Categories;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Catalog;

namespace RoseApp.Application.Features.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler
    : IRequestHandler<GetCategoryByIdQuery, ResponseViewModel<CategoryDto>>
{
    private readonly IDbContext _context;

    public GetCategoryByIdQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<CategoryDto>> Handle(
        GetCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var category = await _context.Set<Category>()
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (category is null)
            throw new NotFoundException(nameof(Category), request.Id);

        return ResponseViewModel<CategoryDto>.Success(new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            PhotoUrl = category.PhotoUrl
        });
    }
}