using MovieCatalogBackend.Data.MovieCatalog;

namespace MovieCatalogBackend.Services.Auth;

public interface ITokenService
{
    public Task RejectToken(string token);
    public Task<bool> IsTokenRejected(string token);
    public string GenerateToken(User user, string issuer, string audience);
}

public static class TokenServiceExtensions
{
    public static string GenerateTokenFor(this ITokenService service, User user, HttpRequest request)
    {
        var selfUrl = string.Format("{0}://{1}", request.Scheme, request.Host);
        return service.GenerateToken(user, selfUrl, selfUrl);
    }
}

