using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Roles.Commands.UpdateRole;

public record UpdateRoleCommand(
    Guid Id,
    string Name,
    string? Description) : IRequest<ResponseViewModel<string>>;