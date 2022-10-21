using Microsoft.EntityFrameworkCore;
using MovieCatalogBackend.Data.Models;

namespace MovieCatalogBackend.Data;

public class MovieCatalogContext : DbContext
{
    public DbSet<User> User { get; set; }
    public DbSet<Movie> Movie { get; set; }
    public DbSet<Review> Review { get; set; }

    public MovieCatalogContext(DbContextOptions options) : base(options) { }

    public static void BuildOptions(IServiceProvider serviceProvider, DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = serviceProvider.GetService<IConfiguration>();
        var connectionString = configuration.GetConnectionString("DefaultConnection")   // Default connection from appsettings.json
            ?? configuration.GetValue<string>("MOVIE_CATALOG_CONN");                    // Environment variable

        if (connectionString == null)
            throw new ArgumentException("Couldn't find connection string for MovieCatalogContext in configuration");

        optionsBuilder.UseOracle(connectionString);
    }
}
