using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Identity;

namespace RoseApp.Application.Features.Roles.Commands.RemoveRoleFromUser;

public class RemoveRoleFromUserCommandHandler
    : IRequestHandler<RemoveRoleFromUserCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public RemoveRoleFromUserCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        RemoveRoleFromUserCommand request,
        CancellationToken cancellationToken)
    {
        var userRole = await _context.Set<UserRole>()
            .FirstOrDefaultAsync(ur => ur.UserId == request.UserId
                && ur.RoleId == request.RoleId, cancellationToken);

        if (userRole is null)
            return ResponseViewModel<string>.Fail(
                "Role not assigned to this user", 404);

        var userRolesCount = await _context.Set<UserRole>()
            .CountAsync(ur => ur.UserId == request.UserId, cancellationToken);

        if (userRolesCount == 1)
            return ResponseViewModel<string>.Fail(
                "User must have at least one role", 400);

        _context.Set<UserRole>().Remove(userRole);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success(
            "Role removed from user successfully");
    }
}