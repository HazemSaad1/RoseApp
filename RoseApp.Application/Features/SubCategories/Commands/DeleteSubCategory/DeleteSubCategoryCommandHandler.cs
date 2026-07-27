using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Catalog;

namespace RoseApp.Application.Features.SubCategories.Commands.DeleteSubCategory;

public class DeleteSubCategoryCommandHandler
    : IRequestHandler<DeleteSubCategoryCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public DeleteSubCategoryCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        DeleteSubCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var subCategory = await _context.Set<SubCategory>()
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (subCategory is null)
            throw new NotFoundException(nameof(SubCategory), request.Id);

        _context.Set<SubCategory>().Remove(subCategory);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("SubCategory deleted successfully");
    }
}