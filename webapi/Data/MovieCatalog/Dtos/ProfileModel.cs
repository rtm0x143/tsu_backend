using System.ComponentModel.DataAnnotations;

namespace MovieCatalogBackend.Data.MovieCatalog.Dtos;

public record ProfileModel
{
    public Guid id { get; set; }
    public string? nickName { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    public string email { get; set; }

    [DataType(DataType.ImageUrl)] public string? avatarLink { get; set; }

    [Required]
    public string? name { get; set; }

    public DateTime? birthDate { get; set; }
    public Gender? gender { get; set; }

    public static explicit operator ProfileModel(User user) => new()
    {
        id = user.Id,
        email = user.Email,
        gender = user.Gender,
        name = user.Name,
        avatarLink = user.Avatar,
        birthDate = user.BirthDate,
        nickName = user.Username
    };

    public User ToUser(Guid userId) => new()
    {
        Id = userId,
        Email = email,
        Gender = gender,
        Name = name,
        Avatar = avatarLink,
        BirthDate = birthDate,
        Username = nickName
    };
}