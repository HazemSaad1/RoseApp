using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Domain.Entities.Catalog;

namespace RoseApp.Application.Features.Occasions.Commands.CreateOccasion;

public class CreateOccasionCommandHandler
    : IRequestHandler<CreateOccasionCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public CreateOccasionCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        CreateOccasionCommand request,
        CancellationToken cancellationToken)
    {
        var exists = await _context.Set<Occasion>()
            .AnyAsync(o => o.Name.ToLower() == request.Name.ToLower(), cancellationToken);

        if (exists)
            return ResponseViewModel<string>.Fail("Occasion already exists", 409);

        var occasion = new Occasion
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Slug = GenerateSlug(request.Name),
            PhotoUrl = request.PhotoUrl
        };

        await _context.Set<Occasion>().AddAsync(occasion, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success(
            "Occasion created successfully", "Created", 201);
    }

    private static string GenerateSlug(string name)
        => name.ToLower().Trim().Replace(" ", "-").Replace("_", "-");
}