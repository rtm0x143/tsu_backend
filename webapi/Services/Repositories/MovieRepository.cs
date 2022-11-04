using Microsoft.EntityFrameworkCore;
using MovieCatalogBackend.Data.MovieCatalog;

namespace MovieCatalogBackend.Services.Repositories;

public class MovieRepository : RepositoryBase, IMovieRepository
{
    private readonly MovieCatalogContext _context;

    public MovieRepository(MovieCatalogContext context, ILogger<MovieRepository> logger) : base(logger) 
        => _context = context;
    
    DbContext IRepository.InnerDbContext => _context;
    public override MovieCatalogContext InnerDbContext => _context;

    public int Count => _context.Movie.Count();
    
    public Movie GetMovieVerbose(Guid id) =>
        _context.Movie
            .Include(m => m.Genres)
            .Include(m => m.Reviews)
            .Include("Reviews.Creator")
            .Single(m => m.Id == id);

    public Movie? this[Guid id]
    {
        get => _context.Movie.Find(id);
        set
        {
            if (value is null)
                _context.Movie.Remove(new Movie { Id = id });
            else
                _context.Movie.Update(value);
        }
    }

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