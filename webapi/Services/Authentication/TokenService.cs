using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using MovieCatalogBackend.Data.MovieCatalog;
using MovieCatalogBackend.Data.Tokens;
using Microsoft.EntityFrameworkCore;

namespace MovieCatalogBackend.Services.Authentication;

public class TokenService : ITokenService
{
    private AuthenticationOptions _authOptions;
    private TokenListContext _tokenListContext;

    public TokenService(AuthenticationOptions options, TokenListContext tokenListContext)
    {
        _authOptions = options;
        _tokenListContext = tokenListContext;
    }

    /// <exception cref="DbUpdateException">see<see cref="DbContext"/> for details</exception>
    public async Task RejectToken(string token)
    {
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        if (jwt.ValidTo < DateTime.UtcNow) return;
         
        _tokenListContext.Tokens.Add(new BlackedToken() { Expiretion = jwt.ValidTo, Token = token });
        await _tokenListContext.SaveChangesAsync();
    }

    public string GenerateToken(User user, string issuer, string audience)
    {
        Claim[] claims = {
            new(ClaimTypes.Sid, user.Id.ToString()),
            new(ClaimTypes.Role, user.Role.ToString())
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
