using MovieCatalogBackend.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddDbContext<MovieCatalogContext>(optionsBuilder =>
    {
        optionsBuilder.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
        

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();