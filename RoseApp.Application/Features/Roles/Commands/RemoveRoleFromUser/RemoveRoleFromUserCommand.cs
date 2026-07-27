using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Roles.Commands.RemoveRoleFromUser;

public record RemoveRoleFromUserCommand(
    Guid UserId,
    Guid RoleId) : IRequest<ResponseViewModel<string>>;