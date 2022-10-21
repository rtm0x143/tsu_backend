using Microsoft.EntityFrameworkCore;
using MovieCatalogBackend.Data.Models;

namespace MovieCatalogBackend.Data;

public class MovieCatalogContext : DbContext
{
    public DbSet<User> User { get; set; }
    public DbSet<Movie> Movie { get; set; }
    public DbSet<Review> Review { get; set; }

    public MovieCatalogContext(DbContextOptions<MovieCatalogContext> options) : base(options) { }
}
