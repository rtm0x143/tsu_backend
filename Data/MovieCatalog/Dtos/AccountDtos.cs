using System.ComponentModel.DataAnnotations;

namespace MovieCatalogBackend.Data.MovieCatalog.Dtos;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class LoginCredentials
{
    [Required] public string username { get; set; }
    [Required] public string password { get; set; }
}

/*different/wierd variables naming is required by api reference*/
public class UserRegisterModel
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

    public static implicit operator User(UserRegisterModel regUser) => new()
    {
        Username = regUser.userName,
        Email = regUser.email,
        Password = regUser.password,
        Name = regUser.name,
        BirthDate = regUser.birthDate != null ? DateTime.Parse(regUser.birthDate) : null,
        Gender = regUser.gender
    };

    public static implicit operator UserRegisterModel(User user) => new()
    {
        userName = user.Username,
        name = user.Name,
        email = user.Email,
        birthDate = user.BirthDate.ToString(),
        password = user.Password,
        gender = user.Gender
    };
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.