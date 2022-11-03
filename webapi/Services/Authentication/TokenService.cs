using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using MovieCatalogBackend.Data.MovieCatalog;
using MovieCatalogBackend.Data.Tokens;

namespace MovieCatalogBackend.Services.Authentication;

public class TokenService : ITokenService
{
    private AuthenticationOptions _authOptions;
    private TokenListContext _tokenListContext;
    private ILogger _logger;

    public TokenService(AuthenticationOptions options, TokenListContext tokenListContext, ILogger<TokenService> logger)
    {
        _authOptions = options;
        _tokenListContext = tokenListContext;
        _logger = logger;
    }

    public async Task RejectToken(string token)
    {
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        if (jwt.ValidTo < DateTime.UtcNow) return;
         
        try
        {
            _tokenListContext.Tokens.Add(new BlackedToken() { Expiretion = jwt.ValidTo, Token = token });
            await _tokenListContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occured while saving rejected token");
        }
    }

    public string GenerateToken(User user, string issuer, string audience)
    {
        Claim[] claims = new[]
        {
            new Claim(ClaimTypes.Sid, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        
        var genTime = DateTime.UtcNow;
        var jwt = new JwtSecurityToken(
            issuer,
            audience,
            claims: claims,
            notBefore: genTime,
            expires: genTime.Add(_authOptions.LiveTime),
            signingCredentials: new SigningCredentials(_authOptions.SecurityKey, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public async Task<bool> IsTokenRejected(string token) => 
        await _tokenListContext.Tokens.FindAsync(token) is not null;
}
