using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Roles.Commands.DeleteRole;

public record DeleteRoleCommand(Guid Id)
    : IRequest<ResponseViewModel<string>>;