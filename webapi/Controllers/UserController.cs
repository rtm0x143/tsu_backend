using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieCatalogBackend.Data.MovieCatalog;
using MovieCatalogBackend.Data.MovieCatalog.Dtos;
using MovieCatalogBackend.Exceptions;
using MovieCatalogBackend.Helpers;
using MovieCatalogBackend.Services.UserServices;

namespace MovieCatalogBackend.Controllers;

[ApiController]
[RequireValidModel]
[Route("api/account")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger _logger;
    private readonly IDbExceptionsHelper _exHelper;
    
    public UserController(IUserService userService, IDbExceptionsHelper exHelper, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
        _exHelper = exHelper;
    }
    
    [Authorize(Policy = "TokenNotBlacked")]
    [HttpGet("profile")]
    public ActionResult<ProfileModel> GetProfile()
    {
        if (!User.TryGetSidAsGuid(out var userId)) return Unauthorized();
        try
        {
            if (_userService[userId] is not User user) return NotFound();
            return (ProfileModel)user;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unknown exception while getting user's({userId}) profile");
            return Problem(title: "Unexpected exception occured");
        }
    }
    
    [Authorize(Policy = "TokenNotBlacked")]
    [HttpPut("profile")]
    public ActionResult PutProfile(ProfileModel model)
    {
        if (!User.TryGetSidAsGuid(out var userId)) return Unauthorized();
        try
        {
            _userService[userId] = model.ToUser(userId);
            return Ok();
        }
        catch (Exception e) when(_exHelper.IsNotFound(e))
        {
            _logger.LogWarning(e, $"Can't update user's({userId}) profile; Seems like couldn't be found");
            return Problem(title: _exHelper.Message, statusCode: StatusCodes.Status404NotFound);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unknown exception while updating user's({userId}) profile");
            return Problem(title: "Unexpected exception occured");
        }
    }
}