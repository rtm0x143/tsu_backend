using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieCatalogBackend.Data.MovieCatalog;
using MovieCatalogBackend.Data.MovieCatalog.Dtos;
using MovieCatalogBackend.Exceptions;
using MovieCatalogBackend.Helpers;

namespace MovieCatalogBackend.Controllers;

[ApiController]
[RequireValidModel]
[Route("api/movie/{movieId:guid}/review")]
public class ReviewController : ControllerBase
{
    private readonly MovieCatalogContext _context;
    private readonly ILogger _logger;
    private readonly IDbExceptionsHelper _exHelper;

    public ReviewController(MovieCatalogContext context, IDbExceptionsHelper exHelper, ILogger<ReviewController> logger)
    {
        _context = context;
        _logger = logger;
        _exHelper = exHelper;
    }

    [Authorize(Policy = "TokenNotBlacked")]
    [Authorize(Policy = "ReviewerPermissions")]
    [HttpPost("add")]
    public async Task<ActionResult> Add(Guid movieId, ReviewModifyModel model)
    {
        if (!User.TryGetSidAsGuid(out var userId)) return Unauthorized();
        try
        {
            _context.Review.Add(model.ToReview(userId, movieId));
            await _context.SaveChangesAsync();
        }
        catch (Exception e) when (_exHelper.IsAlreadyExist(e))
        {
            return Problem(title: _exHelper.Message, detail: "User has already had review for this movie",
                statusCode: StatusCodes.Status409Conflict);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unknown exception while creating review from user({userId}) on movie({movieId})");
            return Problem(title: "Unexpected exception occured");
        }

        return Ok();
    }

    [Authorize(Policy = "TokenNotBlacked")]
    [HttpDelete("{reviewId}/delete")]
    public async Task<ActionResult> Delete(Guid movieId, Guid reviewId)
    {
        if (!User.TryGetSidAsGuid(out var userId)) return Unauthorized();
        try
        {
            _context.Review.Remove(new Review { Id = reviewId });
            await _context.SaveChangesAsync();
            return Ok();
        }
        catch (Exception e) when (_exHelper.IsNotFound(e))
        {
            _logger.LogWarning(e, $"Occured while deleting review({reviewId})");
            return Problem(title: _exHelper.Message, detail: "Unknown review id",
                statusCode: StatusCodes.Status404NotFound);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unknown exception while deleting review from user({userId}) on movie({movieId})");
            return Problem(title: "Unexpected exception occured");
        }
    }

    [Authorize(Policy = "TokenNotBlacked")]
    [HttpPost("{reviewId}/edit")]
    public async Task<ActionResult> Edit(Guid movieId, Guid reviewId, ReviewModifyModel model)
    {
        if (!User.TryGetSidAsGuid(out var userId)) return Unauthorized();
        try
        {
            _context.Review.Update(model.ToReview(userId, movieId, reviewId));
            await _context.SaveChangesAsync();
            return Ok();
        }
        catch (Exception e) when (_exHelper.IsNotFound(e))
        {
            _logger.LogWarning(e, $"Occured while editing review({reviewId})");
            return Problem(title: _exHelper.Message, detail: "Unknown review id",
                statusCode: StatusCodes.Status404NotFound);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unknown exception while editing review from user({userId}) on movie({movieId})");
            return Problem(title: "Unexpected exception occured");
        }
    }
}