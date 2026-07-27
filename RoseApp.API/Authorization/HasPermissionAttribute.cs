using Microsoft.AspNetCore.Authorization;

namespace RoseApp.API.Authorization;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission)
        : base(policy: permission)
    {
    }
}