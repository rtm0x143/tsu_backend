using System.ComponentModel.DataAnnotations;

namespace MovieCatalogBackend.Data.MovieCatalog.Dtos;

public record MovieElementModel
{
    [Required] public Guid id { get; set; }
    public string? name { get; set; }

    [DataType(DataType.ImageUrl)] public string? poster { get; set; }

    public short year { get; set; }
    public string? country { get; set; }
    [Required] public GenreModel[]? genres { get; set; }
    [Required] public ReviewShortModel[]? reviews { get; set; }

    public static explicit operator MovieElementModel(Movie movie) => new()
    {
        id = movie.Id,
        country = movie.Country,
        genres = movie.Genres?.Select(g => (GenreModel)g).ToArray() ?? Array.Empty<GenreModel>(),
        name = movie.Name,
        poster = movie.Poster,
        reviews = movie.Reviews?.Select(r => (ReviewShortModel)r).ToArray() ?? Array.Empty<ReviewShortModel>(),
        year = movie.Year
    };
}