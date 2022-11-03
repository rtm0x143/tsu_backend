using System.ComponentModel.DataAnnotations;

namespace MovieCatalogBackend.Data.Tokens;

public record TokenDto(string token)
{
    [Required]
    public string token { get; set; } = token;
}
