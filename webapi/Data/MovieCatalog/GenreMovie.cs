using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieCatalogBackend.Data.MovieCatalog;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record GenreMovie
{
    [Key] public Guid MovieId { get; set; }
    [Key] public Guid GenreId { get; set; }

    [ForeignKey("MovieId")]
    public Movie Movie { get; set; }
    [ForeignKey("GenreId")]
    public Genre Genre { get; set; }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
