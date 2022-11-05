using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace MovieCatalogBackend.Data.MovieCatalog.Dtos;

public record UserRegisterModel
{
    [Required] public string userName { get; set; }
    [Required] public string name { get; set; }
    [Required] public string password { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    public string email { get; set; }

    [DataType(DataType.Date)]
    public string? birthDate { get; set; }

    public Gender? gender { get; set; }

    public User ToUser(string hashedPassword) => new()
    {
        Username = userName,
        Email = email,
        Password = hashedPassword,
        Name = name,
        BirthDate = birthDate != null ? DateTime.Parse(birthDate) : null,
        Gender = gender
    };
}