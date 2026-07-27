using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Identity;

namespace RoseApp.Application.Features.Roles.Commands.UpdateRole;

public class UpdateRoleCommandHandler
    : IRequestHandler<UpdateRoleCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public UpdateRoleCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        UpdateRoleCommand request,
        CancellationToken cancellationToken)
    {
        var role = await _context.Set<Role>()
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (role is null)
            throw new NotFoundException(nameof(Role), request.Id);

        if (role.Name == "SuperAdmin")
            return ResponseViewModel<string>.Fail(
                "SuperAdmin role cannot be modified", 403);

        var nameExists = await _context.Set<Role>()
            .AnyAsync(r => r.Name.ToLower() == request.Name.ToLower()
                && r.Id != request.Id, cancellationToken);

        if (nameExists)
            return ResponseViewModel<string>.Fail("Role name already exists", 409);

        role.Name = request.Name;
        role.Description = request.Description;

        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Role updated successfully");
    }
}