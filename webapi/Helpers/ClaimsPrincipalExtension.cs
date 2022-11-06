using System.Security.Claims;
using MovieCatalogBackend.Services.Authentication;

namespace MovieCatalogBackend.Helpers;

public static class ClaimsPrincipalExtension
{
    /// <summary>
    /// Tries to get Sid claim from <see cref="claims"/>
    /// </summary>
    /// <remarks>
    /// <see cref="guid"/> = <see cref="Guid.Empty"/>, when not succeed
    /// </remarks>
    /// <returns>true when succeed, false otherwise</returns>
    public static bool TryGetSidAsGuid(this ClaimsPrincipal claims, out Guid guid)
    {
        guid = Guid.Empty;
        return claims.FindFirst(ClaimTypes.Sid)?.Value is string stId 
               && Guid.TryParse(stId, out guid);
    }

    /// <summary>
    /// Tries to get Role claim from <see cref="claims"/>
    /// </summary>
    /// <remarks>
    /// <see cref="role"/> = <see cref="UserRole.None"/>, when not succeed
    /// </remarks>
    /// <returns>true when succeed, false otherwise</returns>
    public static bool TryGetRole(this ClaimsPrincipal claims, out UserRole role)
    {
        role = UserRole.None;
        return claims.FindFirst(ClaimTypes.Role)?.Value is string roleName &&
               Enum.TryParse<UserRole>(roleName, out role);
    }
}