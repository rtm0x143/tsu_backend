using MovieCatalogBackend.Data.MovieCatalog.Dtos;
using Microsoft.AspNetCore.Mvc;
using MovieCatalogBackend.Services.MovieServices;

namespace MovieCatalogBackend.Controllers;

[Route("api/movie")]
public class MovieController : ControllerBase
{
    private readonly IMovieSelector _movieSelector;
    private readonly ILogger _logger;

    public MovieController(IMovieSelector movieSelector, ILogger<MovieController> logger)
    {
        _movieSelector = movieSelector;
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
                    pageCount = (int)Math.Ceiling((double)_movieSelector.Count / pageSize)
                },
                movies = _movieSelector.GetFromMovies((page - 1) * pageSize, pageSize, m => (MovieElementModel)m)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unknown exception while selecting page : {page}, pageSize : {pageSize}",
                page, pageSize);
            return StatusCode(500);
        }
    }

    [HttpGet("details/{id:guid}")]
    public ActionResult<MovieDetailsModel> GetDetails(Guid id)
    {
        try
        {
            return (MovieDetailsModel)_movieSelector.GetMovieVerbose(id);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unknown exception while finding movie with id : {id}", id);
            return StatusCode(500);
        }
    }
}