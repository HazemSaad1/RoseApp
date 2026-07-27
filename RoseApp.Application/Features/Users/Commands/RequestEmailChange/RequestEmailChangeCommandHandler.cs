using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Contracts.IServices;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Identity;
using RoseApp.Domain.Enums;

namespace RoseApp.Application.Features.Users.Commands.RequestEmailChange
{
    public class RequestEmailChangeCommandHandler
     : IRequestHandler<RequestEmailChangeCommand, ResponseViewModel<string>>
    {
        private readonly IDbContext _context;
        private readonly IEmailService _emailService;

        public RequestEmailChangeCommandHandler(
            IDbContext context,
            IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<ResponseViewModel<string>> Handle(
            RequestEmailChangeCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _context.Set<User>()
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user is null)
                throw new NotFoundException(nameof(User), request.UserId);

            var emailExists = await _context.Set<User>()
                .AnyAsync(u => u.Email == request.NewEmail, cancellationToken);

            if (emailExists)
                return ResponseViewModel<string>.Fail("Email is already in use", 409);

            var oldOtps = await _context.Set<OtpVerification>()
                .Where(o => o.Email == request.NewEmail
                    && o.Type == OtpType.EmailChange
                    && !o.IsUsed)
                .ToListAsync(cancellationToken);

            if (oldOtps.Any())
                _context.Set<OtpVerification>().RemoveRange(oldOtps);

            var code = new Random().Next(100000, 999999).ToString();

            var otp = new OtpVerification
            {
                Id = Guid.NewGuid(),
                Email = request.NewEmail,
                Code = code,
                Type = OtpType.EmailChange,
                IsUsed = false,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10)
            };

            await _context.Set<OtpVerification>().AddAsync(otp, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            await _emailService.SendEmailAsync(
                request.NewEmail,
                "Email Change Verification",
                $"Your email change code is: {code}. Valid for 10 minutes.");

            return ResponseViewModel<string>.Success(
                "Verification code sent to your new email");
        }
    }
}