using System.ComponentModel.DataAnnotations;

namespace MovieCatalogBackend.Data.MovieCatalog.Dtos;

public record PageInfoModel
{
    public int pageSize { get; set; }
    public int pageCount { get; set; }
    public int currentPage { get; set; }
}

public record MoviesListModel
{
    [Required] public MovieElementModel[] movies { get; set; }

    public static MoviesListModel From(IEnumerable<Movie> movies) => new()
        { movies = movies.Select(m => (MovieElementModel)m).ToArray() };
}

public record MoviePagedListModel : MoviesListModel
{
    [Required] public PageInfoModel pageInfo { get; set; }
}