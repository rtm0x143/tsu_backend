﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieCatalogBackend.Data.MovieCatalog.Dtos;
using MovieCatalogBackend.Exceptions;
using MovieCatalogBackend.Helpers;
using MovieCatalogBackend.Services.UserServices;

namespace MovieCatalogBackend.Controllers;

[ApiController]
[Route("api/favorites")]
public class FavoritesController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger _logger;

    public FavoritesController(IUserService userService, ILogger<FavoritesController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet]
    [Authorize(Policy = "TokenNotBlacked")]
    public async Task<ActionResult<MoviesListModel>> Get()
    {
        if (!User.SidAsGuid(out var id)) return Unauthorized();
        try
        {
            return await _userService.GetFavoriteMovies(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "While fetching favorites for user : {id}", id);
            return StatusCode(500);
        }
    }

    [Authorize(Policy = "TokenNotBlacked")]
    [HttpPost("{movieId:guid}/add")]
    public async Task<ActionResult> Add(Guid movieId)
    {
        if (!User.SidAsGuid(out var userId)) return Unauthorized();
        try
        {
            await _userService.AddFavoriteMovie(userId, movieId);
            return Ok();
        }
        catch (BadModelException e)
        {
            return Conflict(ProblemDetailsFactory.CreateValidationProblemDetails(HttpContext, e.ModelState,
                StatusCodes.Status409Conflict));
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unknown error while adding favorite movie({movieId}) to user({userId})");
            return StatusCode(500);
        }
    }

    [Authorize(Policy = "TokenNotBlacked")]
    [HttpDelete("{movieId:guid}/delete")]
    public async Task<ActionResult> Delete(Guid movieId)
    {
        if (!User.SidAsGuid(out var userId)) return Unauthorized();
        try
        {
            await _userService.RemoveFavoriteMovie(userId, movieId);
            return Ok();
        }
        catch (BadModelException e)
        {
            _logger.LogWarning(e.Message, e.InnerException);
            return BadRequest(ProblemDetailsFactory.CreateValidationProblemDetails(HttpContext, e.ModelState,
                StatusCodes.Status400BadRequest));
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unknown error while adding favorite movie({movieId}) to user({userId})");
            return StatusCode(500);
        }
    }
}