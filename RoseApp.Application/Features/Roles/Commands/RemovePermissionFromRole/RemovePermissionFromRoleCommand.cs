using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Roles.Commands.RemovePermissionFromRole;

public record RemovePermissionFromRoleCommand(
    Guid RoleId,
    Guid PermissionId) : IRequest<ResponseViewModel<string>>;