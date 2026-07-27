using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Categories;
using RoseApp.Domain.Entities.Catalog;

namespace RoseApp.Application.Features.Categories.Queries.GetAllCategories;

public class GetAllCategoriesQueryHandler
    : IRequestHandler<GetAllCategoriesQuery, ResponseViewModel<PaginationResult<CategoryDto>>>
{
    private readonly IDbContext _context;

    public GetAllCategoriesQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<PaginationResult<CategoryDto>>> Handle(
        GetAllCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Set<Category>().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(c =>
                c.Name.Contains(request.Search) ||
                c.Slug.Contains(request.Search));

        var totalCount = await query.CountAsync(cancellationToken);

        var categories = await query
            .Skip((request.Page - 1) * request.Limit)
            .Take(request.Limit)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                PhotoUrl = c.PhotoUrl
            })
            .ToListAsync(cancellationToken);

        var result = PaginationResult<CategoryDto>.Create(
            categories, totalCount, request.Page, request.Limit);

        return ResponseViewModel<PaginationResult<CategoryDto>>.Success(result);
    }
}