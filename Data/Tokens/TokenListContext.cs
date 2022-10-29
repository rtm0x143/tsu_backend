﻿using Microsoft.EntityFrameworkCore;
using MovieCatalogBackend.Helpers;

namespace MovieCatalogBackend.Data.Tokens;

public class TokenListContext : DbContext
{
    public DbSet<BlackedToken> Tokens { get; set; }

    public TokenListContext() { }
    public TokenListContext(DbContextOptions<TokenListContext> options) : base(options) { }

    public static void BuildOptions(IServiceProvider serviceProvider, DbContextOptionsBuilder optionsBuilder) =>
        BuildOptions(serviceProvider.GetService<IConfiguration>()!, optionsBuilder);

    public static void BuildOptions(IConfiguration configuration, DbContextOptionsBuilder optionsBuilder)
    {
        var connection = configuration.GetConnectionString("TokenList")    // connection from appsettings.json
                          ?? configuration.GetValue<string>("TOKEN_LIST_CONN");           // Environment variable
        
        if (connection == null)
            throw new ArgumentException("Couldn't find connection string for TokenListContext in configuration");

        optionsBuilder.UseOracle(connection);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        BuildOptions(ConfigurationHelper.BaseConfiguration, optionsBuilder);
        base.OnConfiguring(optionsBuilder);
    }
}
