using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Roles;

namespace RoseApp.Application.Features.Roles.Queries.GetAllPermissions;

public record GetAllPermissionsQuery()
    : IRequest<ResponseViewModel<List<PermissionDto>>>;