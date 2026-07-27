using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.SubCategories;
using RoseApp.Domain.Entities.Catalog;

namespace RoseApp.Application.Features.SubCategories.Queries.GetAllSubCategories;

public class GetAllSubCategoriesQueryHandler
    : IRequestHandler<GetAllSubCategoriesQuery, ResponseViewModel<PaginationResult<SubCategoryDto>>>
{
    private readonly IDbContext _context;

    public GetAllSubCategoriesQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<PaginationResult<SubCategoryDto>>> Handle(
        GetAllSubCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Set<SubCategory>()
            .Include(s => s.Category)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(s =>
                s.Name.Contains(request.Search) ||
                s.Slug.Contains(request.Search));

        var totalCount = await query.CountAsync(cancellationToken);

        var subCategories = await query
            .Skip((request.Page - 1) * request.Limit)
            .Take(request.Limit)
            .Select(s => new SubCategoryDto
            {
                Id = s.Id,
                Name = s.Name,
                Slug = s.Slug,
                PhotoUrl = s.PhotoUrl,
                CategoryId = s.CategoryId,
                CategoryName = s.Category.Name
            })
            .ToListAsync(cancellationToken);

        var result = PaginationResult<SubCategoryDto>.Create(
            subCategories, totalCount, request.Page, request.Limit);

        return ResponseViewModel<PaginationResult<SubCategoryDto>>.Success(result);
    }
}