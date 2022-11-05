using System.ComponentModel.DataAnnotations;

namespace MovieCatalogBackend.Data.MovieCatalog.Dtos;

public record MovieDetailsModel : MovieElementModel
{
    public string? description { get; set; }
    public int time { get; set; }
    public string? tagLine { get; set; }
    public string? director { get; set; }
    public int? budget { get; set; }
    public int? fees { get; set; }
    public short ageLimit { get; set; }

    [Required] public new ReviewModel[]? reviews { get; set; }

    public static explicit operator MovieDetailsModel(Movie movie) => new()
    {
        id = movie.Id,
        country = movie.Country,
        genres = movie.Genres?.Select(g => (GenreModel)g).ToArray() ?? Array.Empty<GenreModel>(),
        name = movie.Name,
        poster = movie.Poster,
        reviews = movie.Reviews?.Select(r => (ReviewModel)r).ToArray() ?? Array.Empty<ReviewModel>(),
        year = movie.Year,
        budget = movie.Fees,
        description = movie.Description,
        director = movie.Director,
        fees = movie.Fees,
        time = movie.Time,
        ageLimit = movie.AgeLimit,
        tagLine = movie.TagLine
    };

    public static explicit operator Movie(MovieDetailsModel movieDetails) => new()
    {
        Id = movieDetails.id,
        Budget = movieDetails.budget,
        Country = movieDetails.country,
        Description = movieDetails.description,
        Director = movieDetails.director,
        Fees = movieDetails.fees,
        Genres = movieDetails.genres.Select(g => (Genre)g).ToArray(),
        Name = movieDetails.name,
        Poster = movieDetails.poster,
        Reviews = movieDetails.reviews?.Select(model => model.ToReview(movieDetails.id)).ToArray(),
        Time = movieDetails.time,
        Year = movieDetails.year,
        AgeLimit = movieDetails.ageLimit,
        TagLine = movieDetails.tagLine
    };
}