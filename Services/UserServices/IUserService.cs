using MovieCatalogBackend.Data.MovieCatalog.Dtos;
using MovieCatalogBackend.Exceptions;
using MovieCatalogBackend.Data.MovieCatalog;

namespace MovieCatalogBackend.Services.UserServices;

public interface IUserService
{
    public User? this[Guid id] { get; set; }

    /// <exception cref="BadModelException">
    /// When with that properties can't be created by database 
    /// </exception>
    public ValueTask<User> Register(UserRegisterModel regModel);

    public ValueTask<User?> UserFromLoginCredentials(LoginCredentials credentials);

    /// <exception cref="BadModelException">
    /// When pair of ids is invalid or already added 
    /// </exception>
    public ValueTask AddFavoriteMovie(Guid userId, Guid movieId);
    
    /// <exception cref="BadModelException">
    /// When pair of ids is invalid or haven't been added yet 
    /// </exception>
    public ValueTask RemoveFavoriteMovie(Guid userId, Guid movieId);

    public ValueTask<MoviesListModel> GetFavoriteMovies(Guid id);
}