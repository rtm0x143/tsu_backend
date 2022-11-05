using Microsoft.EntityFrameworkCore;
using MovieCatalogBackend.Helpers;

namespace MovieCatalogBackend.Data.MovieCatalog;

public class MovieCatalogContext : DbContext
{
    public DbSet<User> User { get; set; }
    public DbSet<Movie> Movie { get; set; }
    public DbSet<Review> Review { get; set; }
    public DbSet<Genre> Genre { get; set; }
    public DbSet<FavoriteMovie> FavoriteMovie { get; set; }
    public DbSet<GenreMovie> GenreMovie { get; set; }

    public MovieCatalogContext(DbContextOptions<MovieCatalogContext> options) : base(options) { }
    public MovieCatalogContext() { }

    public static void BuildOptions(IServiceProvider serviceProvider, DbContextOptionsBuilder optionsBuilder) =>
        BuildOptions(serviceProvider.GetService<IConfiguration>()!, optionsBuilder);

    public static void BuildOptions(IConfiguration configuration, DbContextOptionsBuilder optionsBuilder)
    {
        var connection = configuration.GetConnectionString("MovieCatalog")    // connection from appsettings.json
            ?? configuration.GetValue<string>("MOVIE_CATALOG_CONN");           // Environment variable

        if (connection == null)
            throw new ArgumentException("Couldn't find connection string for MovieCatalogContext in configuration");

        optionsBuilder.UseOracle(connection);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        BuildOptions(ConfigurationHelper.BaseConfiguration, optionsBuilder);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FavoriteMovie>().HasKey("MovieId", "UserId");

        modelBuilder.Entity<User>()
            .HasMany(u => u.Favorites)
            .WithMany(m => m.UsersFavored)
            .UsingEntity<FavoriteMovie>();
        
        modelBuilder.Entity<GenreMovie>().HasKey("MovieId", "GenreId");

        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Genres)
            .WithMany(g => g.Movies)
            .UsingEntity<GenreMovie>();
        
        base.OnModelCreating(modelBuilder);
    }
}
