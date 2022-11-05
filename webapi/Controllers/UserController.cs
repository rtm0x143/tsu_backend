using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieCatalogBackend.Data.MovieCatalog;
using MovieCatalogBackend.Data.MovieCatalog.Dtos;
using MovieCatalogBackend.Helpers;
using MovieCatalogBackend.Services.UserServices;

namespace MovieCatalogBackend.Controllers;

[ApiController]
[Route("api/account")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger _logger;
    
    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }
    
    [Authorize(Policy = "TokenNotBlacked")]
    [HttpGet("profile")]
    public ActionResult<ProfileModel> GetProfile()
    {
        if (!User.SidAsGuid(out var userId)) return Unauthorized();
        try
        {
            if (_userService[userId] is not User user) return NotFound();
            return (ProfileModel)user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Unknown exception while getting user's({userId}) profile");
            return Problem(title: "Unexpected exception occured");
        }
    }
    
    [Authorize(Policy = "TokenNotBlacked")]
    [HttpPut("profile")]
    public ActionResult PutProfile(ProfileModel model)
    {
        if (!User.SidAsGuid(out var userId)) return Unauthorized();
        try
        {
            _userService[userId] = model.ToUser(userId);
            return Ok();
        }
        catch (DBConcurrencyException e)
        {
            _logger.LogWarning(e, $"Can't update user's({userId}) profile; Seems like couldn't be found");
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Unknown exception while updating user's({userId}) profile");
            return Problem(title: "Unexpected exception occured");
        }
    }
}