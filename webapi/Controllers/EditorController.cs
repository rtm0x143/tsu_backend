using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieCatalogBackend.Data.MovieCatalog;
using MovieCatalogBackend.Data.MovieCatalog.Dtos;
using MovieCatalogBackend.Exceptions;
using MovieCatalogBackend.Helpers;
using MovieCatalogBackend.Services.Repositories;

namespace MovieCatalogBackend.Controllers;

[ApiController]
[RequireValidModel]
[Route("api/movie")]
public class EditorController : ControllerBase
{
    private readonly IMovieRepository _movieRepository;
    private readonly ILogger _logger;
    private readonly IDbExceptionsHelper _exHelper;

    public EditorController(IMovieRepository repository, IDbExceptionsHelper exHelper, ILogger<EditorController> logger)
    {
        _movieRepository = repository;
        _logger = logger;
        _exHelper = exHelper;
    }

    [HttpPost]
    [Authorize(Policy = "EditorPermissions")]
    [Authorize(Policy = "TokenNotBlacked")]
    public async Task<ActionResult> PostMovie([FromBody] MovieModifyModel model)
    {
        try
        {
            _movieRepository.Add((Movie)model);
            await _movieRepository.FlushChangesAsync();
            User.TryGetSidAsGuid(out var id);
            _logger.LogInformation($"Editor({id}) created new movie with name '{model.name}'");
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unknown exception while creating new movie({model})");
            return Problem(title: "Unexpected exception occured");
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "EditorPermissions")]
    [Authorize(Policy = "TokenNotBlacked")]
    public async Task<ActionResult> PutMovie(Guid id, [FromBody] MovieModifyModel model)
    {
        try
        {
            _movieRepository[id] = (Movie)model;
            await _movieRepository.FlushChangesAsync();
            User.TryGetSidAsGuid(out var userId);
            _logger.LogInformation($"Editor({userId}) modified movie({id})");
            return Ok();
        }
        catch (Exception e) when (_exHelper.IsNotFound(e))
        {
            _logger.LogWarning(e, $"Can't update movie({id}); {_exHelper.Message}");
            return Problem(statusCode: StatusCodes.Status404NotFound, title: _exHelper.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unknown exception while updating movie({model})");
            return Problem(title: "Unexpected exception occured");
        }
    }


    [HttpPatch("{id:guid}/genre/add")]
    [Authorize(Policy = "EditorPermissions")]
    [Authorize(Policy = "TokenNotBlacked")]
    public async Task<ActionResult> AddGenre(Guid id, [Required] GenreAddModel genre)
    {
        try
        {
            await _movieRepository.AddGenre(id, genre);
            await _movieRepository.FlushChangesAsync();
            return Ok();
        }
        catch (InvalidOperationException)
        {
            _logger.LogWarning($"Tried to add unknown genre({genre}) or genre collection was empty'");
            return NotFound();
        }
        catch (Exception e) when (_exHelper.IsAlreadyExist(e))
        {
            _logger.LogInformation($"Tried to add already added genre({genre})");
            return Problem(title: e.Message, detail: "This movie already has that genre",
                statusCode: StatusCodes.Status409Conflict);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unknown exception while adding genre({genre}) to movie({id})");
            return Problem(title: "Unexpected exception occured");
        }
    }
}




