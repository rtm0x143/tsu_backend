using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Http.Json;
using MovieCatalogBackend.Data.MovieCatalog;
using MovieCatalogBackend.Data.MovieCatalog.Dtos;
using MovieCatalogBackend.Helpers;

namespace DatabaseFiller;

internal static class Program
{
    internal static readonly HttpClient Client = new();
    internal static readonly string ApiUrl = "https://react-midterm.kreosoft.space/api";

    static async Task Main()
    {
        var movieIds = new List<Guid>();
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

            movieIds.AddRange(res.movies.Select(m => m.id));
            pageCount = res.pageInfo.pageCount;
        } while (page <= pageCount);

        var users = new HashSet<User>(HasIdEqualityComparer.Instance);
        var genres = new HashSet<Genre>(HasIdEqualityComparer.Instance);
        var reviews = new HashSet<Review>(HasIdEqualityComparer.Instance);
        var moviesWithoutGenres = new HashSet<Movie>(HasIdEqualityComparer.Instance);
        var moviesGenres = new Dictionary<Guid, Genre[]>();

        foreach (var movieId in movieIds)
        {
            MovieDetailsModel? movieDetails = null;
            try
            {
                movieDetails = await Client.GetAsync($"{ApiUrl}/movies/details/{movieId}")
                    .Result
                    .Content
                    .ReadFromJsonAsync<MovieDetailsModel>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Message}; \nOccured when fetching movie id : {movieId}");
            }

            if (movieDetails is null) continue;
            movieDetails.reviews = movieDetails.reviews
                .Where(r => r.author != null)
                .DistinctBy(md => md.id).ToArray();
            movieDetails.genres = movieDetails.genres.DistinctBy(g => g.id).ToArray();

            foreach (var review in movieDetails.reviews)
            {
                var user = review.author;
                users.Add(new User
                {
                    Avatar = user.avatar, Email = user.userId + "@mail.com", Id = user.userId,
                    Username = user.nickName ?? user.userId.ToString(), Password = user.userId.ToString(),
                    Name = "blank"
                });

                reviews.Add(review.ToReview(movieId));
            }

            foreach (var genre in movieDetails.genres)
            {
                genre.name ??= "blank";
                genres.Add((Genre)genre);
            }

            var mov = (Movie)movieDetails;
            moviesGenres.Add(movieDetails.id, mov.Genres?.ToArray() ?? Array.Empty<Genre>());
            mov.Genres = null;
            moviesWithoutGenres.Add(mov);
        }

        await using (var context = new MovieCatalogContext())
        {
            context.User.AddRange(
                users.Where(u => !context.User.Contains(u))
                    .Distinct<User>(HasIdEqualityComparer.Instance)
            );
            await context.SaveChangesAsync();
        }

        await using (var context = new MovieCatalogContext())
        {
            context.Genre.AddRange(
                genres.Where(g => !context.Genre.Contains(g))
                    .Distinct<Genre>(HasIdEqualityComparer.Instance)
            );
            await context.SaveChangesAsync();
        }
        
        await using (var context = new MovieCatalogContext())
        {
            context.Movie.AddRange(
                moviesWithoutGenres.Where(m => !context.Movie.Contains(m))
                    .DistinctBy(m => m.Id)
                );
            await context.SaveChangesAsync();
        }

        // adding relations Genre-Movie
        foreach (var pair in moviesGenres)
        {
            await using var context = new MovieCatalogContext();
            var entity = context.Movie.Find(pair.Key);
            if (entity is null || pair.Value.Length == 0) continue;
            entity.Genres = pair.Value.Where(g => !entity.Genres?.Contains(g) ?? false).ToArray();
            await context.SaveChangesAsync();
        }

        await using (var context = new MovieCatalogContext())
        {
            context.Review.AddRange(
                reviews.Where(r => !context.Review.Contains(r))
                    .Distinct<Review>(HasIdEqualityComparer.Instance)
            );
            await context.SaveChangesAsync();
        }
    }
}