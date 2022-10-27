using System.ComponentModel.DataAnnotations;

namespace MovieCatalogBackend.Data.MovieCatalog.Dtos;

public class PageInfoModel
{
    public int pageSize { get; set; }
    public int pageCount { get; set; }
    public int currentPage { get; set; }
}

public class MoviePagedListModel
{
    [Required] public MovieElementModel[] movies { get; set; }
    [Required] public PageInfoModel pageInfo { get; set; }
}