using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.SubCategories;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Catalog;

namespace RoseApp.Application.Features.SubCategories.Queries.GetSubCategoryById;

public class GetSubCategoryByIdQueryHandler
    : IRequestHandler<GetSubCategoryByIdQuery, ResponseViewModel<SubCategoryDto>>
{
    private readonly IDbContext _context;

    public GetSubCategoryByIdQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<SubCategoryDto>> Handle(
        GetSubCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var subCategory = await _context.Set<SubCategory>()
            .Include(s => s.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (subCategory is null)
            throw new NotFoundException(nameof(SubCategory), request.Id);

        return ResponseViewModel<SubCategoryDto>.Success(new SubCategoryDto
        {
            Id = subCategory.Id,
            Name = subCategory.Name,
            Slug = subCategory.Slug,
            PhotoUrl = subCategory.PhotoUrl,
            CategoryId = subCategory.CategoryId,
            CategoryName = subCategory.Category.Name
        });
    }
}