using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Catalog;

namespace RoseApp.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler
    : IRequestHandler<DeleteCategoryCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public DeleteCategoryCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        DeleteCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var category = await _context.Set<Category>()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (category is null)
            throw new NotFoundException(nameof(Category), request.Id);

        _context.Set<Category>().Remove(category);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Category deleted successfully");
    }
}