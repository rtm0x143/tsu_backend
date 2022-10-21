using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieCatalogBackend.Data.Models;

public class Review
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [Required]
    public Guid MovieId { get; set; }

    [Required]
    public Guid UserId { get; set; }

    public string ReviewText { get; set; } = string.Empty;

    [Required]
    public int Rating { get; set; }

    public bool IsAnonymous { get; set; } = false;

    [DataType(DataType.Date)]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreateDateTime { get; set; }
}