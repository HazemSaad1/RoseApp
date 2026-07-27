using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Roles.Commands.AssignRoleToUser;

public record AssignRoleToUserCommand(
    Guid UserId,
    Guid RoleId) : IRequest<ResponseViewModel<string>>;