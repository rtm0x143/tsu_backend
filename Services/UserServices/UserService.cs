using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieCatalogBackend.Data.MovieCatalog;
using MovieCatalogBackend.Data.MovieCatalog.Dtos;
using MovieCatalogBackend.Exceptions;
using Oracle.ManagedDataAccess.Client;

namespace MovieCatalogBackend.Services.UserServices;

public class UserService : IUserService
{
    private readonly MovieCatalogContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(MovieCatalogContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public User? this[Guid id]
    {
        get => _context.User.Find(id);
        set
        {
            if (value is null)
                _context.User.Remove(new User { Id = id }); 
            else
                _context.User.Update(value);
        }
    }

    public async ValueTask<User> Register(UserRegisterModel regModel)
    {
        try
        {
            var user = regModel.ToUser(_passwordHasher.HashPassword(regModel, regModel.password));
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
        catch (DbUpdateConcurrencyException e) when (e.InnerException is OracleException { Number: 1 })
        {
            var collision = _context.User
                .FirstOrDefault(user => user.Username == regModel.userName || user.Email == regModel.email);
            if (collision is null) throw;

            var exception = new BadModelException(
                "Some fields of given model must be unique but have collisions in database", e.InnerException);
            if (collision.Username == regModel.userName)
                exception.ModelState.AddModelError("userName", "userName field must have be unique");
            exception.ModelState.AddModelError("email", "email field must have be unique");
            throw exception;
        }
    }

    public async ValueTask<User?> UserFromLoginCredentials(LoginCredentials credentials)
    {
        var user = await _context.User.FirstOrDefaultAsync(x => x.Username == credentials.username);
        if (user != null &&
            _passwordHasher
                .VerifyHashedPassword(user, user.Password, credentials.password)
                == PasswordVerificationResult.Success)
            return user;
        return null;
    }

    public async ValueTask AddFavoriteMovie(Guid userId, Guid movieId)
    {
        try
        {
            await _context.FavoriteMovie.AddAsync(new FavoriteMovie { MovieId = movieId, UserId = userId });
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException e) when (e.InnerException is OracleException { Number: 1 } oraEx)
        {
            var newException = new BadModelException(
                $"Relation 'FavoriteMovie' movie({movieId}) to user({userId}) already exist", oraEx);
            newException.ModelState.AddModelError("movieId", "Movie with that id already added");
            throw newException;
        }
        catch (DbUpdateException e) when (e.InnerException is OracleException { Number: 2291 } oraEx)
        {
            var newException = new BadModelException(
                $"Can't create relation between movie({movieId}) and user({userId})", oraEx);
            newException.ModelState.AddModelError("movieId", "Unknown movie id");
            throw newException;
        }
    }

    public async ValueTask RemoveFavoriteMovie(Guid userId, Guid movieId)
    {
        try
        {
            _context.FavoriteMovie.Remove(new FavoriteMovie { MovieId = movieId, UserId = userId });
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException e)
        {
            var newException = new BadModelException(
                $"Can't delete favourite movie({movieId}) from user({userId})", e.InnerException ?? e);
            newException.ModelState.AddModelError("movieId",
                $"That movie doesn't below to user({userId}) favorite films");
            throw newException;
        }
    }

    public async ValueTask<MoviesListModel> GetFavoriteMovies(Guid id) => new()
    {
        movies = await _context.FavoriteMovie
            .Include("Movie.Reviews.Creator")
            .Include("Movie.Genres")
            .Where(fm => fm.UserId == id)
            .Select(fm => (MovieElementModel)fm.Movie)
            .ToArrayAsync()
    };
}