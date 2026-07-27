using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Identity;

namespace RoseApp.Application.Features.Roles.Commands.RemovePermissionFromRole;

public class RemovePermissionFromRoleCommandHandler
    : IRequestHandler<RemovePermissionFromRoleCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public RemovePermissionFromRoleCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        RemovePermissionFromRoleCommand request,
        CancellationToken cancellationToken)
    {
        var rolePermission = await _context.Set<RolePermission>()
            .FirstOrDefaultAsync(rp => rp.RoleId == request.RoleId
                && rp.PermissionId == request.PermissionId, cancellationToken);

        if (rolePermission is null)
            return ResponseViewModel<string>.Fail(
                "Permission not assigned to this role", 404);

        _context.Set<RolePermission>().Remove(rolePermission);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success(
            "Permission removed from role successfully");
    }
}