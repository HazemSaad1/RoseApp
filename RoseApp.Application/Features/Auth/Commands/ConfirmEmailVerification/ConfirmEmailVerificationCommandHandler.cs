using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Domain.Entities.Identity;
using RoseApp.Domain.Enums;

namespace RoseApp.Application.Features.Auth.Commands.ConfirmEmailVerification
{
    public class ConfirmEmailVerificationCommandHandler
       : IRequestHandler<ConfirmEmailVerificationCommand, ResponseViewModel<string>>
    {
        private readonly IDbContext _context;

        public ConfirmEmailVerificationCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel<string>> Handle(
            ConfirmEmailVerificationCommand request,
            CancellationToken cancellationToken)
        {
            var otp = await _context.Set<OtpVerification>()
                .FirstOrDefaultAsync(o =>
                    o.Email == request.Email
                    && o.Code == request.Code
                    && o.Type == OtpType.EmailVerification
                    && !o.IsUsed,
                cancellationToken);

            if (otp is null)
                return ResponseViewModel<string>.Fail("Invalid or expired code", 400);

            if (otp.ExpiresAt < DateTime.UtcNow)
                return ResponseViewModel<string>.Fail("Code has expired", 400);

            otp.IsUsed = true;
            await _context.SaveChangesAsync(cancellationToken);

            return ResponseViewModel<string>.Success("Email verified successfully");
        }
    }
}