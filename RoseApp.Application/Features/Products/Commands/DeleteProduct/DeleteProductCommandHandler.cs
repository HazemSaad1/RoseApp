using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Catalog;

namespace RoseApp.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler
    : IRequestHandler<DeleteProductCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public DeleteProductCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        DeleteProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _context.Set<Product>()
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (product is null)
            throw new NotFoundException(nameof(Product), request.Id);

        product.IsDeleted = true;
        product.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Product deleted successfully");
    }
}   