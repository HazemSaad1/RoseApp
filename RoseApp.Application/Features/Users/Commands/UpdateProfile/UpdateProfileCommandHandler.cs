using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Identity;

namespace RoseApp.Application.Features.Users.Commands.UpdateProfile
{
    public class UpdateProfileCommandHandler
       : IRequestHandler<UpdateProfileCommand, ResponseViewModel<string>>
    {
        private readonly IDbContext _context;

        public UpdateProfileCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel<string>> Handle(
            UpdateProfileCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _context.Set<User>()
                .FindAsync(new object?[] { request.UserId }, cancellationToken);

            if (user is null)
                throw new NotFoundException(nameof(User), request.UserId);

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;

            if (request.PhotoUrl is not null)
                user.PhotoUrl = request.PhotoUrl;

            await _context.SaveChangesAsync(cancellationToken);

            return ResponseViewModel<string>.Success("Profile updated successfully");
        }
    }
}