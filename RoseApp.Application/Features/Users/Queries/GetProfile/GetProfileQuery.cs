using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Users;

namespace RoseApp.Application.Features.Users.Queries.GetProfile
{
    public record GetProfileQuery(Guid UserId)
       : IRequest<ResponseViewModel<UserProfileDto>>;
}
