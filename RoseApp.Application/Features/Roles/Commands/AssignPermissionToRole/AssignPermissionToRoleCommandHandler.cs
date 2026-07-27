using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Identity;

namespace RoseApp.Application.Features.Roles.Commands.AssignPermissionToRole;

public class AssignPermissionToRoleCommandHandler
    : IRequestHandler<AssignPermissionToRoleCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public AssignPermissionToRoleCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        AssignPermissionToRoleCommand request,
        CancellationToken cancellationToken)
    {
        var role = await _context.Set<Role>()
            .FirstOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);

        if (role is null)
            throw new NotFoundException(nameof(Role), request.RoleId);

        var permission = await _context.Set<Permission>()
            .FirstOrDefaultAsync(p => p.Id == request.PermissionId, cancellationToken);

        if (permission is null)
            throw new NotFoundException(nameof(Permission), request.PermissionId);

        var alreadyAssigned = await _context.Set<RolePermission>()
            .AnyAsync(rp => rp.RoleId == request.RoleId
                && rp.PermissionId == request.PermissionId, cancellationToken);

        if (alreadyAssigned)
            return ResponseViewModel<string>.Fail(
                "Permission already assigned to this role", 409);

        await _context.Set<RolePermission>().AddAsync(new RolePermission
        {
            RoleId = request.RoleId,
            PermissionId = request.PermissionId
        }, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success(
            "Permission assigned to role successfully");
    }
}