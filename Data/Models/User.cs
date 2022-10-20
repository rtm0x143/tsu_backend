using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieCatalogBackend.Data.Models;

public enum Gender : byte
{
    Female,
    Male
}

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string? Name { get; set; }

    public DateOnly BirthDate { get; set; }

    [Required(AllowEmptyStrings = false)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required(AllowEmptyStrings = false)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool IsAdmin { get; set; } = false;

    public Gender Gender { get; set; }

    [DataType(DataType.Url)]
    public string? Avatar { get; set; }

    public List<Review> Reviews { get; set; }
}
