using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieCatalogBackend.Data.MovieCatalog;

public enum Gender : byte
{
    Female,
    Male
}

public enum UserRole : byte
{
    User,
    Admin
}

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public UserRole Role { get; set; } = UserRole.User;

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

    public ICollection<Review> Reviews { get; set; }

    public ICollection<Movie> Favorites { get; set; }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.