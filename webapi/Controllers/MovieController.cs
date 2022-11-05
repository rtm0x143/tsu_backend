using System.Data;
using Microsoft.AspNetCore.Authorization;
using MovieCatalogBackend.Data.MovieCatalog.Dtos;
using Microsoft.AspNetCore.Mvc;
using MovieCatalogBackend.Data.MovieCatalog;
using MovieCatalogBackend.Services.Repositories;

namespace MovieCatalogBackend.Controllers;

[Route("api/movies")]
public class MovieController : ControllerBase
{
    private readonly IMovieRepository _movieRepository;
    private readonly ILogger _logger;

    public MovieController(IMovieRepository movieRepository, ILogger<MovieController> logger)
    {
        _movieRepository = movieRepository;
        _logger = logger;
    }

    [HttpGet("{page:int}")]
    public ActionResult<MoviePagedListModel> GetPage(int page, int pageSize = 5)
    {
        if (page < 1)
            ModelState.AddModelError("page", "The page field value can't be less than 1");
        if (pageSize < 1)
            ModelState.AddModelError("pageSize", "The pageSize value can't be less than 1");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            return new MoviePagedListModel
            {
                pageInfo = new()
                {
                    pageSize = pageSize,
                    currentPage = page,
                    pageCount = (int)Math.Ceiling((double)_movieRepository.Count / pageSize)
                },
                movies = _movieRepository.GetFromMovies((page - 1) * pageSize, pageSize, m => (MovieElementModel)m)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Unknown exception while selecting page : {page}, pageSize : {pageSize}");
            return Problem(title: "Unexpected exception occured");
        }
    }

    [HttpGet("details/{id:guid}")]
    public ActionResult<MovieDetailsModel> GetDetails(Guid id)
    {
        try
        {
            return (MovieDetailsModel)_movieRepository.GetMovieVerbose(id);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Unknown exception while finding movie with id : {id}");
            return Problem(title: "Unexpected exception occured");
        }
    }

    [HttpPost]
    [Authorize(Policy = "EditorPermissions")]
    [Authorize(Policy = "TokenNotBlacked")]
    public async Task<ActionResult> PostMovie([FromBody] MovieCreationModel details)
    {
        try
        {
            _movieRepository.Add((Movie)details);
            await _movieRepository.FlushChangesAsync();
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unknown exception while creating new movie({details})");
            return Problem(title: "Unexpected exception occured");
        }
    }
    
    [HttpPut]
    [Authorize(Policy = "EditorPermissions")]
    [Authorize(Policy = "TokenNotBlacked")]
    public async Task<ActionResult> PutMovie([FromBody] MovieDetailsModel model)
    {
        try
        {
            _movieRepository[model.id] = (Movie)model;
            await _movieRepository.FlushChangesAsync();
            return Ok();
        }
        catch (DBConcurrencyException e)
        {
            _logger.LogWarning(e, $"Can't update movie({model.id}). Seems like not found");
            return NotFound();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unknown exception while updating movie({model})");
            return Problem(title: "Unexpected exception occured");
        }
    }
}