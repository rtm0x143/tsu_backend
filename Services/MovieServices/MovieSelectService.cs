using Microsoft.EntityFrameworkCore;
using MovieCatalogBackend.Data.MovieCatalog;

namespace MovieCatalogBackend.Services.MovieServices;

public class MovieSelectService : IMovieSelector
{
    private readonly MovieCatalogContext _context;
    public MovieSelectService(MovieCatalogContext context)
    {
        _context = context;
    }

    public int Count => _context.Movie.Count();
    
    public Movie GetMovieVerbose(Guid id) =>
        _context.Movie
            .Include(m => m.Genres)
            .Include(m => m.Reviews)
            .Include("Reviews.Creator")
            .Single(m => m.Id == id);
    public IQueryable<Movie> FetchSomeMovies(int begin, int amount) =>
        _context.Movie
            .Include(m => m.Genres)
            .Include(m => m.Reviews)
            .Skip(begin)
            .Take(amount);

    public TModel[] GetFromMovies<TModel>(int begin, int amount, Func<Movie, TModel> selector) =>
        FetchSomeMovies(begin, amount)
            // ReSharper disable once PossibleUnintendedQueryableAsEnumerable
            .Select(selector)
            .ToArray();

    public Movie[] GetFromMovies(int begin, int amount) => FetchSomeMovies(begin, amount).ToArray();
}