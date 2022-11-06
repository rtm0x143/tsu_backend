using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace MovieCatalogBackend.Services.Auth;

/// <summary>
/// Builds policies for pattern /[<see cref="UserPrivilegeMask"/>]Permissions/ 
/// </summary>
public class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    private DefaultAuthorizationPolicyProvider BackupPolicyProvider { get; }
    public AuthorizationPolicy DefaultPolicy { get; set; } =
        new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser().Build();

    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options) =>
        BackupPolicyProvider = new DefaultAuthorizationPolicyProvider(options);


    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.EndsWith("Permissions") &&
            Enum.TryParse<UserPrivilegeMask>(policyName.Substring(0, policyName.Length - 11), out var privilege))
        {
            var builder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .AddRequirements(new UserPrivilegeRequirement(privilege));
            return Task.FromResult<AuthorizationPolicy?>(builder.Build());
        }

        return BackupPolicyProvider.GetPolicyAsync(policyName);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => Task.FromResult(DefaultPolicy);
    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => BackupPolicyProvider.GetFallbackPolicyAsync();
}