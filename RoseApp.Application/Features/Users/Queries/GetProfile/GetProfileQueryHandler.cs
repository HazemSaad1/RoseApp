using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Users;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Identity;

namespace RoseApp.Application.Features.Users.Queries.GetProfile
{
    public class GetProfileQueryHandler
     : IRequestHandler<GetProfileQuery, ResponseViewModel<UserProfileDto>>
    {
        private readonly IDbContext _context;

        public GetProfileQueryHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel<UserProfileDto>> Handle(
            GetProfileQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _context.Set<User>()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user is null)
                throw new NotFoundException(nameof(User), request.UserId);

            var roleIds = await _context.Set<UserRole>()
                .Where(ur => ur.UserId == user.Id)
                .Select(ur => ur.RoleId)
                .ToListAsync(cancellationToken);

            var roles = await _context.Set<Role>()
                .Where(r => roleIds.Contains(r.Id))
                .ToListAsync(cancellationToken);

            var dto = new UserProfileDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                PhotoUrl = user.PhotoUrl,
                Roles = roles.Select(r => r.Name).ToList()
            };

            return ResponseViewModel<UserProfileDto>.Success(dto);
        }
    }
}