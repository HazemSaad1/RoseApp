using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace RoseApp.API.Authorization;

public class PermissionAuthorizationHandler
    : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var permissions = context.User
            .Claims
            .Where(c => c.Type == "permission")
            .Select(c => c.Value)
            .ToList();

        if (permissions.Contains(requirement.Permission))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}