using System.ComponentModel.DataAnnotations;

namespace MovieCatalogBackend.Data.MovieCatalog.Dtos;

public record MovieModifyModel
{
    public string? description { get; set; }
    public string? country { get; set; }
    public int time { get; set; }
    public string? tagLine { get; set; }
    public string? director { get; set; }
    public int? budget { get; set; }
    public int? fees { get; set; }
    public short ageLimit { get; set; }
    public short year { get; set; }
    [Required] public string name { get; set; }
    [DataType(DataType.ImageUrl)] public string? poster { get; set; }

    public static explicit operator Movie(MovieModifyModel model) => new()
    {
        Budget = model.budget,
        Country = model.country,
        Description = model.description,
        Director = model.director,
        Fees = model.fees,
        Name = model.name,
        Time = model.time,
        Year = model.year,
        AgeLimit = model.ageLimit,
        TagLine = model.tagLine,
        Poster = model.poster
    };
}