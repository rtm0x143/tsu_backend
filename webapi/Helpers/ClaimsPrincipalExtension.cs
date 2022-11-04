using System.Security.Claims;

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
    public static bool SidAsGuid(this ClaimsPrincipal claims, out Guid guid)
    {
        if (claims.FindFirst(ClaimTypes.Sid)?.Value is string stId)
        {
            guid = new Guid(stId);
            return true;
        }
        guid = Guid.Empty;
        return false;
    }  
}