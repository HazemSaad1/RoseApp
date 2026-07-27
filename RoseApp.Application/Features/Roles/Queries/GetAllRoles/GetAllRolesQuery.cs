using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Roles;

namespace RoseApp.Application.Features.Roles.Queries.GetAllRoles;

public record GetAllRolesQuery()
    : IRequest<ResponseViewModel<List<RoleDto>>>;