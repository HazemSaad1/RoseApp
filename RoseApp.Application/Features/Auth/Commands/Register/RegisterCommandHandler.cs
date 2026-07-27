using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Domain.Entities.Identity;
using RoseApp.Domain.Enums;

namespace RoseApp.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler
        : IRequestHandler<RegisterCommand, ResponseViewModel<string>>
    {
        private readonly IDbContext _context;

        public RegisterCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel<string>> Handle(
            RegisterCommand request,
            CancellationToken cancellationToken)
        {
            var verifiedOtp = await _context.Set<OtpVerification>()
                .AnyAsync(o =>
                    o.Email == request.Email
                    && o.Type == OtpType.EmailVerification
                    && o.IsUsed,
                cancellationToken);

            if (!verifiedOtp)
                return ResponseViewModel<string>.Fail(
                    "Email not verified. Please verify your email first", 400);

            var emailExists = await _context.Set<User>()
                .AnyAsync(u => u.Email == request.Email, cancellationToken);

            if (emailExists)
                return ResponseViewModel<string>.Fail("Email is already registered", 409);

            var userNameExists = await _context.Set<User>()
                .AnyAsync(u => u.UserName == request.UserName, cancellationToken);

            if (userNameExists)
                return ResponseViewModel<string>.Fail("Username is already taken", 409);

            var customerRole = await _context.Set<Role>()
                .FirstOrDefaultAsync(r => r.Name == "Customer", cancellationToken);

            if (customerRole is null)
                return ResponseViewModel<string>.ServerError("Default role not found");

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                IsEmailVerified = true
            };

            await _context.Set<User>().AddAsync(user, cancellationToken);

            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = customerRole.Id
            };

            await _context.Set<UserRole>().AddAsync(userRole, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return ResponseViewModel<string>.Success(
                "Registration successful",
                "Account created successfully",
                201);
        }
    }
}