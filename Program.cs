using MovieCatalogBackend.Data.MovieCatalog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using MovieCatalogBackend.Data.Tokens;
using MovieCatalogBackend.Helpers;
using MovieCatalogBackend.Services;
using MovieCatalogBackend.Services.Authentication;

var builder = WebApplication.CreateBuilder(args);
ConfigurationHelper.BaseConfiguration = builder.Configuration;

builder.Services.AddControllers();

var authenticationOptions = new AuthenticationOptions(builder.Configuration);
var serverUrls = builder.WebHost.GetSetting(WebHostDefaults.ServerUrlsKey)?.Split(";").ToHashSet();
if (serverUrls != null)
{
    authenticationOptions.Issuers.UnionWith(serverUrls);
    authenticationOptions.Audiences.UnionWith(serverUrls);
}

builder.Services
    .AddSingleton(authenticationOptions) 
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => 
        options.TokenValidationParameters = authenticationOptions.CreateValidationParameters());

builder.Services
    .AddScoped<ITokenService, TokenService>()
    .AddSingleton<TokenListCleanerDemon>()
    .AddScoped<IAuthorizationHandler, TokenNotBlackedAuthorizationHandler>()
    .AddAuthorization(options =>
        options.AddPolicy("TokenNotBlacked", policyBuilder => policyBuilder.AddRequirements(TokenNotBlackedRequirements.Istance))
    );

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

// Startup cleaner  
app.Services.GetService<TokenListCleanerDemon>();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();