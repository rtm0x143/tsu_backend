using MovieCatalogBackend.Data.MovieCatalog;

namespace MovieCatalogBackend.Services.MovieServices;

public interface IMovieSelector
{
    public int Count { get; }
    
    /// <summary>
    /// Finds movie in database and includes all related information
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// No movie satisfies the id or more than one element satisfies if or database is empty
    /// </exception>
    public Movie GetMovieVerbose(Guid id);

    /// <summary>
    /// Gets bunch of movies with not deep reviews data 
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
}