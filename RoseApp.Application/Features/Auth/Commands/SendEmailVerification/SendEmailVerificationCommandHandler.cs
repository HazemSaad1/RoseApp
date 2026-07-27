using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Contracts.IServices;
using RoseApp.Domain.Entities.Identity;
using RoseApp.Domain.Enums;

namespace RoseApp.Application.Features.Auth.Commands.SendEmailVerification
{
    public class SendEmailVerificationCommandHandler
       : IRequestHandler<SendEmailVerificationCommand, ResponseViewModel<string>>
    {
        private readonly IDbContext _context;
        private readonly IEmailService _emailService;

        public SendEmailVerificationCommandHandler(
            IDbContext context,
            IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<ResponseViewModel<string>> Handle(
            SendEmailVerificationCommand request,
            CancellationToken cancellationToken)
        {
            var existingUser = await _context.Set<User>()
                .AnyAsync(u => u.Email == request.Email, cancellationToken);

            if (existingUser)
                return ResponseViewModel<string>.Fail("Email is already registered", 409);

            var oldOtps = await _context.Set<OtpVerification>()
                .Where(o => o.Email == request.Email
                    && o.Type == OtpType.EmailVerification
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
                Type = OtpType.EmailVerification,
                IsUsed = false,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10)
            };

            await _context.Set<OtpVerification>().AddAsync(otp, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            await _emailService.SendEmailAsync(
                request.Email,
                "Email Verification",
                $"Your verification code is: {code}. Valid for 10 minutes.");

            return ResponseViewModel<string>.Success(
                "OTP sent successfully",
                "Please check your email");
        }

        private static string GenerateOtp()
            => new Random().Next(100000, 999999).ToString();
    }
}