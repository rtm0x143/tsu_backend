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
    public Guid Id { get; set; }

    [Required]
    [MaxLength(64)]
    public string Username { get; set; }

    [Required]
    [MaxLength(64)]
    public string Name { get; set; }

    public DateTime? BirthDate { get; set; }

    [Required(AllowEmptyStrings = false)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required(AllowEmptyStrings = false)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool IsAdmin { get; set; } = false;

    public Gender? Gender { get; set; }

    [DataType(DataType.ImageUrl)]
    public string? Avatar { get; set; }
}
