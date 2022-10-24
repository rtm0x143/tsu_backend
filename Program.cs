using MovieCatalogBackend.Data.MovieCatalog;
using MovieCatalogBackend.Data.Tokens;
using MovieCatalogBackend.Helpers;

var builder = WebApplication.CreateBuilder(args);
ConfigurationHelper.BaseConfiguration = builder.Configuration;

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddDbContext<MovieCatalogContext>(MovieCatalogContext.BuildOptions)
    .AddDbContext<TokenListContext>(TokenListContext.BuildOptions);
        
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();