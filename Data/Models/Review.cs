using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieCatalogBackend.Data.Models;

public class Review
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public int MovieId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public string ReviewText { get; set; }

    public int Rating { get; set; }

    public bool IsAnonymous { get; set; } = false;

    [DataType(DataType.Date)]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreateDateTime { get; set; }
}