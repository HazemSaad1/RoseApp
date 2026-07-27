using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Contracts.IServices;
using RoseApp.Application.DTOs.Auth;
using RoseApp.Domain.Entities.Identity;

namespace RoseApp.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler
       : IRequestHandler<LoginCommand, ResponseViewModel<AuthResponseDto>>
    {
        private readonly IDbContext _context;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(
            IDbContext context,
            ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<ResponseViewModel<AuthResponseDto>> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _context.Set<User>()
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (user is null)
                return ResponseViewModel<AuthResponseDto>.Fail("Invalid credentials", 401);

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return ResponseViewModel<AuthResponseDto>.Fail("Invalid credentials", 401);

            var roleIds = await _context.Set<UserRole>()
                .Where(ur => ur.UserId == user.Id)
                .Select(ur => ur.RoleId)
                .ToListAsync(cancellationToken);

            var roles = await _context.Set<Role>()
                .Where(r => roleIds.Contains(r.Id))
                .ToListAsync(cancellationToken);

            var token = await _tokenService.GenerateTokenAsync(user);

            var response = new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles.Select(r => r.Name).ToList()
            };

            return ResponseViewModel<AuthResponseDto>.Success(response, "Login successful");
        }
    }
}