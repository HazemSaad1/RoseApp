using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Identity;
using RoseApp.Domain.Enums;

namespace RoseApp.Application.Features.Users.Commands.ConfirmEmailChange
{
    public class ConfirmEmailChangeCommandHandler
      : IRequestHandler<ConfirmEmailChangeCommand, ResponseViewModel<string>>
    {
        private readonly IDbContext _context;

        public ConfirmEmailChangeCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel<string>> Handle(
            ConfirmEmailChangeCommand request,
            CancellationToken cancellationToken)
        {
            var otp = await _context.Set<OtpVerification>()
                .FirstOrDefaultAsync(o => o.Email == request.NewEmail
                    && o.Code == request.Code
                    && o.Type == OtpType.EmailChange
                    && !o.IsUsed,
                cancellationToken);

            if (otp is null)
                return ResponseViewModel<string>.Fail("Invalid or expired code", 400);

            if (otp.ExpiresAt < DateTime.UtcNow)
                return ResponseViewModel<string>.Fail("Code has expired", 400);

            var user = await _context.Set<User>()
                .FindAsync(new object?[] { request.UserId }, cancellationToken);

            if (user is null)
                throw new NotFoundException(nameof(User), request.UserId);

            user.Email = request.NewEmail;

            otp.IsUsed = true;

            await _context.SaveChangesAsync(cancellationToken);

            return ResponseViewModel<string>.Success("Email changed successfully");
        }
    }
}