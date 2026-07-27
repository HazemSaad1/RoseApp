using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Roles.Commands.AssignPermissionToRole;

public record AssignPermissionToRoleCommand(
    Guid RoleId,
    Guid PermissionId) : IRequest<ResponseViewModel<string>>;