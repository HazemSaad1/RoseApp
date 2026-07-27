using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Domain.Entities.Identity;

namespace RoseApp.Application.Features.Roles.Commands.CreateRole;

public class CreateRoleCommandHandler
    : IRequestHandler<CreateRoleCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public CreateRoleCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        CreateRoleCommand request,
        CancellationToken cancellationToken)
    {
        var exists = await _context.Set<Role>()
            .AnyAsync(r => r.Name.ToLower() == request.Name.ToLower(),
                cancellationToken);

        if (exists)
            return ResponseViewModel<string>.Fail("Role already exists", 409);

        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description
        };

        await _context.Set<Role>().AddAsync(role, cancellationToken);

        if (request.PermissionIds.Any())
        {
            var validPermissions = await _context.Set<Permission>()
                .Where(p => request.PermissionIds.Contains(p.Id))
                .ToListAsync(cancellationToken);

            var rolePermissions = validPermissions.Select(p => new RolePermission
            {
                RoleId = role.Id,
                PermissionId = p.Id
            });

            await _context.Set<RolePermission>().AddRangeAsync(
                rolePermissions, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success(
            "Role created successfully", "Created", 201);
    }
}