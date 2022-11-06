using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace MovieCatalogBackend.Services.Auth;

public class TokenNotBlackedRequirements : IAuthorizationRequirement
{
    protected TokenNotBlackedRequirements() { }
    public static TokenNotBlackedRequirements Instance = new();
}

public class TokenNotBlackedAuthorizationHandler : AuthorizationHandler<TokenNotBlackedRequirements, HttpContext>
{
    ITokenService _tokenService;

    public TokenNotBlackedAuthorizationHandler(ITokenService tokenService) =>
        _tokenService = tokenService;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TokenNotBlackedRequirements requirement, HttpContext resource)
    {
        var token = await resource.GetTokenAsync("access_token");
        if (token is null) context.Fail();
        else if (await _tokenService.IsTokenRejected(token))
            context.Fail(new AuthorizationFailureReason(this, "Token is rejected, that could mean that session had logged out"));
        else context.Succeed(requirement);
    }
}