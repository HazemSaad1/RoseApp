using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Catalog;

namespace RoseApp.Application.Features.Occasions.Commands.UpdateOccasion;

public class UpdateOccasionCommandHandler
    : IRequestHandler<UpdateOccasionCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public UpdateOccasionCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        UpdateOccasionCommand request,
        CancellationToken cancellationToken)
    {
        var occasion = await _context.Set<Occasion>()
            .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

        if (occasion is null)
            throw new NotFoundException(nameof(Occasion), request.Id);

        var nameExists = await _context.Set<Occasion>()
            .AnyAsync(o => o.Name.ToLower() == request.Name.ToLower()
                && o.Id != request.Id, cancellationToken);

        if (nameExists)
            return ResponseViewModel<string>.Fail("Occasion name already exists", 409);

        occasion.Name = request.Name;
        occasion.Slug = GenerateSlug(request.Name);

        if (request.PhotoUrl is not null)
            occasion.PhotoUrl = request.PhotoUrl;

        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Occasion updated successfully");
    }

    private static string GenerateSlug(string name)
        => name.ToLower().Trim().Replace(" ", "-").Replace("_", "-");
}