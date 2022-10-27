using System.ComponentModel.DataAnnotations;

namespace MovieCatalogBackend.Data.MovieCatalog.Dtos;

public class ReviewModifyModel
{
    [Required] public string reviewText { get; set; }
    
    [Range(0, 10, ErrorMessage = "rating can only take values in [0, 10]")] 
    public int rating { get; set; }
    
    public bool isAnonymous { get; set; } 
}