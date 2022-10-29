using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MovieCatalogBackend.Data.MovieCatalog;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

[Index("TargetMovieId", "CreatorId", IsUnique = true)] 
public class Review : IHasGuid
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid TargetMovieId { get; set; }
    [ForeignKey("TargetMovieId")] public Movie TargetMovie { get; set; }

    public Guid CreatorId { get; set; }
    [ForeignKey("CreatorId")] public User Creator { get; set; }

    public string ReviewText { get; set; } = string.Empty;

    [Required] public int Rating { get; set; }

    public bool IsAnonymous { get; set; } = false;

    [DataType(DataType.Date)] public DateTime? CreateDateTime { get; set; } = DateTime.UtcNow;
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.