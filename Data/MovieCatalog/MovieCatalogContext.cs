using Microsoft.EntityFrameworkCore;
using MovieCatalogBackend.Helpers;

namespace MovieCatalogBackend.Data.MovieCatalog;

public class MovieCatalogContext : DbContext
{
    public DbSet<User> User { get; set; }
    public DbSet<Movie> Movie { get; set; }
    public DbSet<Review> Review { get; set; }

    public MovieCatalogContext(DbContextOptions<MovieCatalogContext> options) : base(options) { }
    public MovieCatalogContext() : base() { }

    public static void BuildOptions(IServiceProvider serviceProvider, DbContextOptionsBuilder optionsBuilder) =>
        BuildOptions(serviceProvider.GetService<IConfiguration>()!, optionsBuilder);

    public static void BuildOptions(IConfiguration configuration, DbContextOptionsBuilder optionsBuilder)
    {
        var _connection = configuration.GetConnectionString("MovieCatalog")    // connection from appsettings.json
            ?? configuration.GetValue<string>("MOVIE_CATALOG_CONN");           // Environment variable

        if (_connection == null)
            throw new ArgumentException("Couldn't find connection string for MovieCatalogContext in configuration");

        optionsBuilder.UseOracle(_connection);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        BuildOptions(ConfigurationHelper.BaseConfiguration, optionsBuilder);
        base.OnConfiguring(optionsBuilder);
    }
}
