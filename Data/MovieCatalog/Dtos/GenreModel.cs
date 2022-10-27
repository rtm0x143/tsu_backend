using System.ComponentModel.DataAnnotations;

namespace MovieCatalogBackend.Data.MovieCatalog.Dtos;

public class GenreModel
{
    [Required] public Guid id { get; set; }
    public string? name { get; set; }

    public static implicit operator GenreModel(Genre genre) => new() { id = genre.Id,
        name = genre.Name };

    public static explicit operator Genre(GenreModel genre) => new()
    { Id = genre.id,
        Name = genre.name ?? throw new ArgumentNullException(nameof(name)) };
}