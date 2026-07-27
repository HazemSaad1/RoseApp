using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Catalog;

namespace RoseApp.Application.Features.Products.Commands.RestoreProduct;

public class RestoreProductCommandHandler
    : IRequestHandler<RestoreProductCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public RestoreProductCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        RestoreProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _context.Set<Product>()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.Id == request.Id && p.IsDeleted,
                cancellationToken);

        if (product is null)
            throw new NotFoundException(nameof(Product), request.Id);

        product.IsDeleted = false;
        product.DeletedAt = null;

        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Product restored successfully");
    }
}