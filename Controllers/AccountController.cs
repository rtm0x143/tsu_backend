using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MovieCatalogBackend.Data.MovieCatalog;
using MovieCatalogBackend.Data.Tokens;
using Microsoft.AspNetCore.Authentication;
using MovieCatalogBackend.Data.MovieCatalog.Dtos;
using MovieCatalogBackend.Services.Authentication;

namespace MovieCatalogBackend.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    readonly ITokenService _tokenService;
    readonly MovieCatalogContext _context;
    readonly ILogger _logger;

    public AccountController(MovieCatalogContext context, ITokenService tokenService, ILogger<AccountController> logger)
    {
        _context = context;
        _tokenService = tokenService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(UserRegisterModel regUser) 
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        try
        {
            var collision = _context.User
                .FirstOrDefault(user => user.Username == regUser.userName || user.Email == regUser.email);

            if (collision != null)
                return Conflict(collision.Email == regUser.email ? "Email already used" : "Username already used");

            _context.User.Add(regUser);
            await _context.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Occured while registration user : {user}", regUser);
            return StatusCode(500);
        }
    }

    [HttpPost("login")]
    public ActionResult<TokenDto> Login(LoginCredentials credentials)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        try
        {
            var user = _context.User
                .FirstOrDefault(x => x.Username == credentials.username && x.Password == credentials.password);
            if (user == null) return NotFound();

            var token = _tokenService.GenerateTokenFor(user, Request);
            _logger.LogInformation("Generated token for user with id : {id}", user.Id);

            return Ok(new TokenDto(token));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unknown exception while authorizing with input ({})", credentials);
            return StatusCode(500);
        }
    }

    [Authorize(Policy = "TokenNotBlacked")]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        if (token is null) return Unauthorized();
        await _tokenService.RejectToken(token);
        return Ok();
    }
}
