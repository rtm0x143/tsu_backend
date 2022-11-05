using System.ComponentModel.DataAnnotations;

namespace MovieCatalogBackend.Data.Tokens;

public record BlackedToken
{
    [Key]
    public string Token { get; set; }

    [Required]
    public DateTime Expiretion { get; set; }
}
