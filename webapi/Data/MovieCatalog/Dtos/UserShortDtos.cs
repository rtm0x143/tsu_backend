using System.ComponentModel.DataAnnotations;

namespace MovieCatalogBackend.Data.MovieCatalog.Dtos;

public record LoginCredentials
{
    [Required] public string username { get; set; }
    [Required] public string password { get; set; }
}

public record UserShortModel
{
    public Guid userId { get; set; }
    public string? nickName { get; set; }
    
    [DataType(DataType.ImageUrl)]
    public string? avatar { get; set; }

    public static explicit operator UserShortModel(User user) =>
        new() { avatar = user.Avatar, nickName = user.Username, userId = user.Id };
}