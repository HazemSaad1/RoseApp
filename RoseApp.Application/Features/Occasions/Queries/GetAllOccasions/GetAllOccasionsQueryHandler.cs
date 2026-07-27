using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Occasions;
using RoseApp.Domain.Entities.Catalog;

namespace RoseApp.Application.Features.Occasions.Queries.GetAllOccasions;

public class GetAllOccasionsQueryHandler
    : IRequestHandler<GetAllOccasionsQuery, ResponseViewModel<PaginationResult<OccasionDto>>>
{
    private readonly IDbContext _context;

    public GetAllOccasionsQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<PaginationResult<OccasionDto>>> Handle(
        GetAllOccasionsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Set<Occasion>().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(o =>
                o.Name.Contains(request.Search) ||
                o.Slug.Contains(request.Search));

        var totalCount = await query.CountAsync(cancellationToken);

        var occasions = await query
            .Skip((request.Page - 1) * request.Limit)
            .Take(request.Limit)
            .Select(o => new OccasionDto
            {
                Id = o.Id,
                Name = o.Name,
                Slug = o.Slug,
                PhotoUrl = o.PhotoUrl
            })
            .ToListAsync(cancellationToken);

        var result = PaginationResult<OccasionDto>.Create(
            occasions, totalCount, request.Page, request.Limit);

        return ResponseViewModel<PaginationResult<OccasionDto>>.Success(result);
    }
}