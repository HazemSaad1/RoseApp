using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Catalog;

namespace RoseApp.Application.Features.Occasions.Commands.DeleteOccasion;

public class DeleteOccasionCommandHandler
    : IRequestHandler<DeleteOccasionCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public DeleteOccasionCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        DeleteOccasionCommand request,
        CancellationToken cancellationToken)
    {
        var occasion = await _context.Set<Occasion>()
            .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

        if (occasion is null)
            throw new NotFoundException(nameof(Occasion), request.Id);

        _context.Set<Occasion>().Remove(occasion);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Occasion deleted successfully");
    }
}