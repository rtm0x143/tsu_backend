using System.ComponentModel.DataAnnotations;

namespace MovieCatalogBackend.Data.MovieCatalog.Dtos;

public record GenreAddModel : IValidatableObject
{
    public string? name { get; set; }
    public Guid? id { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (id == null && name == null)
            yield return new ValidationResult("name field and id field can't be null at the same time",
                new[] { "name", "id" });
    }
}