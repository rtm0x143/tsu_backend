using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieCatalogBackend.Data.MovieCatalog;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class Movie
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [MaxLength(256)]
    public string Name { get; set; }

    [DataType(DataType.ImageUrl)]
    public string? Poster { get; set; }

    public string Description { get; set; } = string.Empty;

    public short Year { get; set; }

    public string? Country { get; set; }

    public int Time { get; set; }

    public string? TagLine { get; set; }

    public string? Director { get; set; }

    public int? Bubget { get; set; }

    public int? Fees { get; set; }

    public short AgeLimit { get; set; }

    public ICollection<Genre> Genres { get; set; }

    public ICollection<Review> Reviews { get; set; }

    public ICollection<User> UsersFavored { get; set; }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
