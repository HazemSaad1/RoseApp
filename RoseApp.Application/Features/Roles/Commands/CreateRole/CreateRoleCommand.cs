using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Roles.Commands.CreateRole;

public record CreateRoleCommand(
    string Name,
    string? Description,
    List<Guid> PermissionIds) : IRequest<ResponseViewModel<string>>;