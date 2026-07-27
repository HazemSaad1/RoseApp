using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Roles;
using RoseApp.Domain.Entities.Identity;

namespace RoseApp.Application.Features.Roles.Queries.GetAllRoles;

public class GetAllRolesQueryHandler
    : IRequestHandler<GetAllRolesQuery, ResponseViewModel<List<RoleDto>>>
{
    private readonly IDbContext _context;

    public GetAllRolesQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<List<RoleDto>>> Handle(
        GetAllRolesQuery request,
        CancellationToken cancellationToken)
    {
        var roles = await _context.Set<Role>()
            .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var dtos = roles.Select(r => new RoleDto
        {
            Id = r.Id,
            Name = r.Name,
            Description = r.Description,

            Permissions = r.RolePermissions
                .Select(rp => rp.Permission.Name)
                .ToList()

        }).ToList();

        return ResponseViewModel<List<RoleDto>>.Success(dtos);
    }
}