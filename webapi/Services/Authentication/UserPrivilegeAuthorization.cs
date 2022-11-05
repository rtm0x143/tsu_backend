using System.Collections.Immutable;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MovieCatalogBackend.Helpers;

namespace MovieCatalogBackend.Services.Authentication;

public class UserPrivilegeRequirement : IAuthorizationRequirement
{
    public readonly ImmutableArray<UserPrivilegeMask> Privileges;
    public UserPrivilegeRequirement(params UserPrivilegeMask[] privileges)
    {
        Privileges = privileges.ToImmutableArray();
    }
}

public class UserPrivilegeAuthorizationHandler : AuthorizationHandler<UserPrivilegeRequirement>
{
    public UserPrivilegeAuthorizationHandler() { }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserPrivilegeRequirement requirement)
    {
        if (context.User.FindFirst(ClaimTypes.Role)?.Value is not string roleName ||
            !Enum.TryParse<UserRole>(roleName, out var role))
        {
            context.Fail(new AuthorizationFailureReason(this, "Invalid role or role hadn't been specified"));
            return Task.CompletedTask;
        }

        foreach (var privilege in requirement.Privileges)
        {
            if (role.HasPrivilege(privilege)) continue;
            context.User.SidAsGuid(out var id);
            context.Fail(new AuthorizationFailureReason(this, $"Privilege {privilege.ToString()} not granted for user({id})"));
            return Task.CompletedTask;
        }
        
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}