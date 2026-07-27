using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Domain.Entities.Identity;
using RoseApp.Domain.Enums;

namespace RoseApp.Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler
     : IRequestHandler<ResetPasswordCommand, ResponseViewModel<string>>
    {
        private readonly IDbContext _context;

        public ResetPasswordCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel<string>> Handle(
            ResetPasswordCommand request,
            CancellationToken cancellationToken)
        {
            var otp = await _context.Set<OtpVerification>()
                .FirstOrDefaultAsync(o =>
                    o.Email == request.Email
                    && o.Code == request.Code
                    && o.Type == OtpType.PasswordReset
                    && !o.IsUsed,
                cancellationToken);

            if (otp is null)
                return ResponseViewModel<string>.Fail("Invalid or expired code", 400);

            if (otp.ExpiresAt < DateTime.UtcNow)
                return ResponseViewModel<string>.Fail("Code has expired", 400);

            var user = await _context.Set<User>()
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (user is null)
                return ResponseViewModel<string>.Fail("User not found", 404);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            otp.IsUsed = true;

            await _context.SaveChangesAsync(cancellationToken);

            return ResponseViewModel<string>.Success("Password reset successfully");
        }
    }
}