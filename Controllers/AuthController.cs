﻿using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MovieCatalogBackend.Data.Tokens;
using Microsoft.AspNetCore.Authentication;
using MovieCatalogBackend.Data.MovieCatalog;
using MovieCatalogBackend.Data.MovieCatalog.Dtos;
using MovieCatalogBackend.Exceptions;
using MovieCatalogBackend.Helpers;
using MovieCatalogBackend.Services.Authentication;
using MovieCatalogBackend.Services.UserServices;

namespace MovieCatalogBackend.Controllers;

[ApiController]
[Route("api/account")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;
    private readonly ILogger _logger;

    public AuthController(IUserService userService, ITokenService tokenService, ILogger<AuthController> logger)
    {
        _userService = userService;
        _tokenService = tokenService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<ActionResult<TokenDto>> Register(UserRegisterModel regModel)
    {
        try
        {
            return Ok(_tokenService.GenerateTokenFor(await _userService.Register(regModel), Request));
        }
        catch (BadModelException e)
        {
            return Conflict(ProblemDetailsFactory.CreateValidationProblemDetails(HttpContext, e.ModelState,
                StatusCodes.Status409Conflict));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Occured while registration user : {user}", regModel);
            return StatusCode(500);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> Login(LoginCredentials credentials)
    {
        try
        {
            var user = await _userService.UserFromLoginCredentials(credentials);
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