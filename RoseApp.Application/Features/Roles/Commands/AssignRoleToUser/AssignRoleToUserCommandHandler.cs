using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Identity;

namespace RoseApp.Application.Features.Roles.Commands.AssignRoleToUser;

public class AssignRoleToUserCommandHandler
    : IRequestHandler<AssignRoleToUserCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public AssignRoleToUserCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        AssignRoleToUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Set<User>()
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
            throw new NotFoundException(nameof(User), request.UserId);

        var role = await _context.Set<Role>()
            .FirstOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);

        if (role is null)
            throw new NotFoundException(nameof(Role), request.RoleId);

        var alreadyAssigned = await _context.Set<UserRole>()
            .AnyAsync(ur => ur.UserId == request.UserId
                && ur.RoleId == request.RoleId, cancellationToken);

        if (alreadyAssigned)
            return ResponseViewModel<string>.Fail(
                "Role already assigned to this user", 409);

        await _context.Set<UserRole>().AddAsync(new UserRole
        {
            UserId = request.UserId,
            RoleId = request.RoleId
        }, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success(
            "Role assigned to user successfully");
    }
}