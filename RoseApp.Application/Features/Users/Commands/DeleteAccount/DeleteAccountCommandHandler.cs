using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Identity;

namespace RoseApp.Application.Features.Users.Commands.DeleteAccount
{
    public class DeleteAccountCommandHandler
      : IRequestHandler<DeleteAccountCommand, ResponseViewModel<string>>
    {
        private readonly IDbContext _context;

        public DeleteAccountCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel<string>> Handle(
            DeleteAccountCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _context.Set<User>()
                .FindAsync(new object?[] { request.UserId }, cancellationToken);

            if (user is null)
                throw new NotFoundException(nameof(User), request.UserId);

            var userRoles = await _context.Set<UserRole>()
                .Where(ur => ur.UserId == user.Id)
                .ToListAsync(cancellationToken);

            var roleIds = userRoles.Select(ur => ur.RoleId).ToList();

            var roles = await _context.Set<Role>()
                .Where(r => roleIds.Contains(r.Id))
                .ToListAsync(cancellationToken);

            if (roles.Any(r => r.Name == "SuperAdmin"))
                return ResponseViewModel<string>.Fail(
                    "SuperAdmin account cannot be deleted", 403);

            _context.Set<User>().Remove(user);
            await _context.SaveChangesAsync(cancellationToken);

            return ResponseViewModel<string>.Success("Account deleted successfully");
        }
    }
}