using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCatalogBackend.Data.MovieCatalog;
using MovieCatalogBackend.Data.MovieCatalog.Dtos;
using MovieCatalogBackend.Helpers;
using Oracle.ManagedDataAccess.Client;

namespace MovieCatalogBackend.Controllers;

[ApiController]
[Route("api/movie/{movieId:guid}/review")]
public class ReviewController : ControllerBase
{
    private readonly MovieCatalogContext _context;
    private readonly ILogger _logger;

    public ReviewController(MovieCatalogContext context, ILogger<ReviewController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [Authorize(Policy = "TokenNotBlacked")]
    [HttpPost("add")]
    public async Task<ActionResult> Add(Guid movieId, ReviewModifyModel model)
    {
        if (!User.SidAsGuid(out var userId)) return Unauthorized();
        try
        {
            _context.Review.Add(model.ToReview(userId, movieId));
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException e) when (e.InnerException is OracleException { Number: 1 })
        {
            return Conflict(ProblemDetailsFactory.CreateProblemDetails(HttpContext, StatusCodes.Status409Conflict,
                detail: "User has already had review for this movie"));
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unknown exception while creating review from user({userId}) on movie({movieId})");
            return StatusCode(500);
        }

        return Ok();
    }

    [Authorize(Policy = "TokenNotBlacked")]
    [HttpDelete("{reviewId}/delete")]
    public async Task<ActionResult> Delete(Guid movieId, Guid reviewId)
    {
        try
        {
            _context.Review.Remove(new Review { Id = reviewId }); 
            await _context.SaveChangesAsync();
            return Ok();
        }
        catch (DbUpdateConcurrencyException e)
        {
            _logger.LogWarning(e, $"Occured while deleting review({reviewId})");
            return NotFound(ProblemDetailsFactory.CreateProblemDetails(HttpContext, StatusCodes.Status404NotFound,
                detail: "Unknown review id"));
        }
        catch (Exception e)
        {
            User.SidAsGuid(out var userId);
            _logger.LogError(e, $"Unknown exception while deleting review from user({userId}) on movie({movieId})");
            return StatusCode(500);
        }
    }

    [Authorize(Policy = "TokenNotBlacked")]
    [HttpPost("{reviewId}/edit")]
    public async Task<ActionResult> Edit(Guid movieId, Guid reviewId, ReviewModifyModel model)
    {
        if (User.SidAsGuid(out var userId)) return Unauthorized();
        try
        {
            _context.Review.Update(model.ToReview(userId, movieId, reviewId));
            await _context.SaveChangesAsync();
            return Ok();
        }
        catch (DbUpdateConcurrencyException e)
        {
            _logger.LogWarning(e, $"Occured while editing review({reviewId})");
            return NotFound(ProblemDetailsFactory.CreateProblemDetails(HttpContext, StatusCodes.Status404NotFound,
                detail: "Unknown review id"));
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unknown exception while editing review from user({userId}) on movie({movieId})");
            return StatusCode(500);
        }
    }
}