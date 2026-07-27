using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Users.Commands.UpdateProfile
{
    public record UpdateProfileCommand(
        Guid UserId,
        string FirstName,
        string LastName,
        string? PhoneNumber,
        string? PhotoUrl) : IRequest<ResponseViewModel<string>>;
}
