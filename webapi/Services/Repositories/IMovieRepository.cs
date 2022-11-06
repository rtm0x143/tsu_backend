using MovieCatalogBackend.Data.MovieCatalog;
using MovieCatalogBackend.Data.MovieCatalog.Dtos;

namespace MovieCatalogBackend.Services.Repositories;

public interface IMovieRepository : IRepository
{
    public new MovieCatalogContext InnerDbContext { get; }
    public int Count { get; }
    
    /// <summary>
    /// Finds movie in database and includes <see cref="Movie.Genres"/>, <see cref="Movie.Reviews"/>, <see cref="Review.Creator"/>
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// No movie satisfies the id or more than one element satisfies if or database is empty
    /// </exception>
    public Movie GetMovieVerbose(Guid id);
    
    public Movie? this[Guid id] { get; set; }

    public void Add(Movie model);

    /// <summary>
    /// Gets bunch of movies with <see cref="Movie.Genres"/> and <see cref="Movie.Reviews"/> includes 
    /// </summary>
    /// <param name="begin">index to begin selection</param>
    /// <param name="amount">amount of movies to select</param>
    public Movie[] GetFromMovies(int begin, int amount);
    
    /// <summary>
    /// Gets bunch of movies with not deep reviews data 
    /// </summary>
    /// <param name="begin">index to begin selection</param>
    /// <param name="amount">amount of movies to select</param>
    /// <param name="selector">your custom selector to pick data from Movie</param>
    public TResult[] GetFromMovies<TResult>(int begin, int amount, Func<Movie, TResult> selector);

    /// <exception cref="InvalidOperationException">When can't find <paramref name="genre"/> in database</exception>
    public ValueTask AddGenre(Guid movieId, GenreAddModel genre);
}