using System.ComponentModel.DataAnnotations;

namespace MovieCatalogBackend.Data.Tokens;

public class BlackedToken
{
    [Key]
    public string Token { get; set; }

    [Required]
    public DateTime Expiretion { get; set; }
}
