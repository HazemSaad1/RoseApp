using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Contracts.IServices;
using RoseApp.Domain.Entities.Identity;
using RoseApp.Domain.Enums;

namespace RoseApp.Application.Features.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler
    : IRequestHandler<ForgotPasswordCommand, ResponseViewModel<string>>
    {
        private readonly IDbContext _context;
        private readonly IEmailService _emailService;

        public ForgotPasswordCommandHandler(
            IDbContext context,
            IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<ResponseViewModel<string>> Handle(
            ForgotPasswordCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _context.Set<User>()
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (user is null)
                return ResponseViewModel<string>.Success(
                    "If this email exists, you will receive a reset link");

            var oldOtps = await _context.Set<OtpVerification>()
                .Where(o => o.Email == request.Email
                    && o.Type == OtpType.PasswordReset
                    && !o.IsUsed)
                .ToListAsync(cancellationToken);

            if (oldOtps.Count != 0)
                _context.Set<OtpVerification>().RemoveRange(oldOtps);

            var code = GenerateOtp();

            var otp = new OtpVerification
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Code = code,
                Type = OtpType.PasswordReset,
                IsUsed = false,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15)
            };

            await _context.Set<OtpVerification>().AddAsync(otp, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            await _emailService.SendEmailAsync(
                request.Email,
                "Password Reset",
                $"Your password reset code is: {code}. Valid for 15 minutes.");

            return ResponseViewModel<string>.Success(
                "If this email exists, you will receive a reset code");
        }

        private static string GenerateOtp()
            => new Random().Next(100000, 999999).ToString();
    }
}