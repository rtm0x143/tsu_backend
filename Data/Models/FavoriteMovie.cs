using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MovieCatalogBackend.Data.Models;

/// <summary>
/// Explicit table of relationships for better selection when you have user's id only
/// </summary>
[Keyless]
public class FavoriteMovie
{
    [Required]
    public Guid UsetId { get; set; }
    [Required]
    public Movie Movie { get; set; }
}