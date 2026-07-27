using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Identity;

namespace RoseApp.Application.Features.Users.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler
     : IRequestHandler<ChangePasswordCommand, ResponseViewModel<string>>
    {
        private readonly IDbContext _context;

        public ChangePasswordCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel<string>> Handle(
            ChangePasswordCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _context.Set<User>()
                .FindAsync(new object?[] { request.UserId }, cancellationToken);

            if (user is null)
                throw new NotFoundException(nameof(User), request.UserId);

            if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
                return ResponseViewModel<string>.Fail("Current password is incorrect", 400);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            await _context.SaveChangesAsync(cancellationToken);

            return ResponseViewModel<string>.Success("Password changed successfully");
        }
    }
}