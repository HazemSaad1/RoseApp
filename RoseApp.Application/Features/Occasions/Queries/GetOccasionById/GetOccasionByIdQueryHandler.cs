using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Occasions;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Catalog;

namespace RoseApp.Application.Features.Occasions.Queries.GetOccasionById;

public class GetOccasionByIdQueryHandler
    : IRequestHandler<GetOccasionByIdQuery, ResponseViewModel<OccasionDto>>
{
    private readonly IDbContext _context;

    public GetOccasionByIdQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<OccasionDto>> Handle(
        GetOccasionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var occasion = await _context.Set<Occasion>()
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

        if (occasion is null)
            throw new NotFoundException(nameof(Occasion), request.Id);

        return ResponseViewModel<OccasionDto>.Success(new OccasionDto
        {
            Id = occasion.Id,
            Name = occasion.Name,
            Slug = occasion.Slug,
            PhotoUrl = occasion.PhotoUrl
        });
    }
}