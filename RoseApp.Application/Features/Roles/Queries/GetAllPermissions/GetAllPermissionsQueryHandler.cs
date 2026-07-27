using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Roles;
using RoseApp.Domain.Entities.Identity;

namespace RoseApp.Application.Features.Roles.Queries.GetAllPermissions;

public class GetAllPermissionsQueryHandler
    : IRequestHandler<GetAllPermissionsQuery, ResponseViewModel<List<PermissionDto>>>
{
    private readonly IDbContext _context;

    public GetAllPermissionsQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<List<PermissionDto>>> Handle(
        GetAllPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var permissions = await _context.Set<Permission>()
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .Select(p => new PermissionDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description
            })
            .ToListAsync(cancellationToken);

        return ResponseViewModel<List<PermissionDto>>.Success(permissions);
    }
}