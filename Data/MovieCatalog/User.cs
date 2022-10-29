using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MovieCatalogBackend.Data.MovieCatalog;

public enum Gender : byte
{
    Female,
    Male
}

public enum UserRole : byte
{
    User = 0x0F,
    Admin = 0xFF
}

public enum UserPrivilegeMask : byte
{
    Admin = 0b10000000,
    User = 0b0001000
}

public static class UserPrivilege
{
    public static bool HasPrivilege(UserRole role, UserPrivilegeMask privilege) => ((byte)role & (byte)privilege) > 0;
}

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

[Index("Username", IsUnique = true)]
[Index("Email", IsUnique = true)]
public class User : IHasGuid
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public UserRole Role { get; set; } = UserRole.User;

    [Required(AllowEmptyStrings = false)]
    [MaxLength(64)]
    public string? Username { get; set; }

    [Required]
    [MaxLength(64)]
    public string? Name { get; set; }

    public DateTime? BirthDate { get; set; }

    [MaxLength(64)]
    [Required(AllowEmptyStrings = false)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [NotMapped]
    public virtual bool IsAdmin 
    { 
        get => ((byte)Role & (byte)UserPrivilegeMask.Admin) > 0;
        set
        {
            if (value == IsAdmin) return;
            Role = (UserRole)((byte)Role + (value ? 1 : -1) * (byte)UserPrivilegeMask.Admin);
        }
    }

    public Gender? Gender { get; set; }

    [DataType(DataType.ImageUrl)]
    [MaxLength(8000)]
    public string? Avatar { get; set; }

    public ICollection<Review>? Reviews { get; set; }

    public ICollection<Movie>? Favorites { get; set; }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.