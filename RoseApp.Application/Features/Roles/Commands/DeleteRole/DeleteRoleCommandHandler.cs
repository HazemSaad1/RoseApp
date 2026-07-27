using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Identity;

namespace RoseApp.Application.Features.Roles.Commands.DeleteRole;

public class DeleteRoleCommandHandler
    : IRequestHandler<DeleteRoleCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public DeleteRoleCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        DeleteRoleCommand request,
        CancellationToken cancellationToken)
    {
        var role = await _context.Set<Role>()
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (role is null)
            throw new NotFoundException(nameof(Role), request.Id);

        if (role.Name is "SuperAdmin" or "Admin" or "Customer")
            return ResponseViewModel<string>.Fail(
                "Core roles cannot be deleted", 403);

        _context.Set<Role>().Remove(role);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Role deleted successfully");
    }
}