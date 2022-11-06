using System.Diagnostics;
using System.Net.Http.Json;
using MovieCatalogBackend.Data.MovieCatalog;
using MovieCatalogBackend.Data.MovieCatalog.Dtos;
using MovieCatalogBackend.Helpers;
using MovieCatalogBackend.Services.Auth;

namespace DatabaseFiller;

internal static class Program
{
    internal static readonly HttpClient Client = new();
    internal static readonly string ApiUrl = "https://react-midterm.kreosoft.space/api";

    internal static async Task<IEnumerable<MovieDetailsModel>> FetchMovieDetails()
    {
        var movies = new HashSet<MovieDetailsModel>();
        int page = 1, pageCount = 10;
        do
        {
            MoviePagedListModel? res = null;
            try
            {
                res = await Client.GetAsync($"{ApiUrl}/movies/{page++}")
                    .Result
                    .Content
                    .ReadFromJsonAsync<MoviePagedListModel>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Message}; \n Occured when fetching page : {page}");
            }

            if (res is null) continue;
            pageCount = res.pageInfo.pageCount;

            foreach (var movie in res.movies)
            {
                MovieDetailsModel? movieDetails = null;
                try
                {
                    movieDetails = await Client.GetAsync($"{ApiUrl}/movies/details/{movie.id}")
                        .Result
                        .Content
                        .ReadFromJsonAsync<MovieDetailsModel>();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"{ex.Message}; \nOccured when fetching movie id : {movie.id}");
                }

                if (movieDetails is null) continue;
                movieDetails.reviews = movieDetails.reviews
                    ?.Where(r => r.author != null)
                    .DistinctBy(md => md.id).ToArray() ?? Array.Empty<ReviewModel>();
                movieDetails.genres = movieDetails.genres?.DistinctBy(g => g.id).ToArray();

                movies.Add(movieDetails);
            }
        } while (page <= pageCount);

        return movies.DistinctBy(md => md.id);
    }

    internal static Dictionary<Guid, Genre[]> PopGenres(IEnumerable<Movie> movies)
    {
        var movieGenres = new Dictionary<Guid, Genre[]>(movies.Count());
        foreach (var movie in movies)
        {
            if (movie.Genres is null) continue;
            movieGenres.Add(movie.Id, movie.Genres.DistinctBy(g => g.Id).ToArray());
            movie.Genres = null;
        }

        return movieGenres;
    }

    internal static Review[] ExtractReviews(IEnumerable<Movie> movies)
    {
        var reviews = new List<Review>();
        foreach (var movie in movies)
            if (movie.Reviews != null)
                reviews.AddRange(movie.Reviews);
        return reviews.DistinctBy(r => r.Id).ToArray();
    }

    internal static User[] ExtractUsers(IEnumerable<MovieDetailsModel> movieDetails, IPasswordHasher hasher)
    {
        var authors = new HashSet<User>(HasIdEqualityComparer.Instance);
        foreach (var movieDetail in movieDetails)
        {
            if (movieDetail.reviews is null) continue;
            foreach (var review in movieDetail.reviews)
            {
                var user = review.author;
                authors.Add(new User
                {
                    Avatar = user.avatar,
                    Email = user.userId + "@mail.com",
                    Id = user.userId,
                    Username = user.nickName ?? user.userId.ToString(),
                    Password = hasher.HashPassword(user, user.userId.ToString()),
                    Name = "blank"
                });
            }
        }

        return authors.ToArray();
    }

    static async Task Main()
    {
        var movieDetails = await FetchMovieDetails();
        var movies = movieDetails.Select(md => (Movie)md).ToArray();
        var movieGenres = PopGenres(movies);

        var genres = new HashSet<Genre>();
        foreach (var someGenres in movieGenres.Values) genres.UnionWith(someGenres);

        var reviews = ExtractReviews(movies);
        var users = ExtractUsers(movieDetails, new SimplePasswordHasher());

        var context = new MovieCatalogContext();

        context.User.AddRange(users);
        await context.SaveChangesAsync();

        context.Genre.AddRange(genres);
        await context.SaveChangesAsync();

        context.Movie.AddRange(movies);
        await context.SaveChangesAsync();

        // adding relations Genre-Movie
        foreach (var pair in movieGenres)
        {
            var entity = context.Movie.Find(pair.Key);
            if (entity is null || pair.Value.Length == 0) continue;

            foreach (var genre in pair.Value)
                context.GenreMovie.Add(new GenreMovie { GenreId = genre.Id, MovieId = entity.Id });
        }

        await context.SaveChangesAsync();
    }
}