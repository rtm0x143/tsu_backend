using MovieCatalogBackend.Data.MovieCatalog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using MovieCatalogBackend.Data.Tokens;
using MovieCatalogBackend.Exceptions;
using MovieCatalogBackend.Helpers;
using MovieCatalogBackend.Services;
using MovieCatalogBackend.Services.Auth;
using MovieCatalogBackend.Services.Repositories;
using MovieCatalogBackend.Services.UserServices;

var builder = WebApplication.CreateBuilder(args);
ConfigurationHelper.BaseConfiguration = builder.Configuration;

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddControllers();

if (builder.Environment.IsProduction())
{
    if (builder.Configuration.GetValue<string>("LaunchSettings:applicationUrl") is string urls)
        builder.WebHost.UseUrls(urls);
}

var authenticationOptions = new AuthenticationOptions(builder.Configuration);
var serverUrls = builder.WebHost.GetSetting(WebHostDefaults.ServerUrlsKey)?.Split(";").ToHashSet();
if (serverUrls != null)
{
    authenticationOptions.Issuers.UnionWith(serverUrls);
    authenticationOptions.Audiences.UnionWith(serverUrls);
}

// Authentication
builder.Services
    .AddSingleton(authenticationOptions) 
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => 
        options.TokenValidationParameters = authenticationOptions.CreateValidationParameters());

// Authorization
builder.Services
    .AddScoped<ITokenService, TokenService>()
    .AddSingleton<TokenListCleanerDemon>()
    .AddSingleton<IPasswordHasher, SimplePasswordHasher>()
    .AddScoped<IAuthorizationHandler, TokenNotBlackedAuthorizationHandler>()
    .AddScoped<IAuthorizationHandler, UserPrivilegeAuthorizationHandler>()
    .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
    .AddSingleton<IAuthorizationMiddlewareResultHandler, AuthorizationResultTransformer>()
    .AddAuthorization(options =>
        {
            options.AddPolicy("TokenNotBlacked", policyBuilder =>
            {
                policyBuilder.RequireAuthenticatedUser()
                    .AddRequirements(TokenNotBlackedRequirements.Instance);
            });
        }
    );

// DB contexts
builder.Services
    .AddDbContext<MovieCatalogContext>(opBuilder =>
    {
        opBuilder.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
        MovieCatalogContext.BuildOptions(builder.Configuration, opBuilder);
    })
    .AddDbContext<TokenListContext>(opBuilder =>
    {
        opBuilder.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
        TokenListContext.BuildOptions(builder.Configuration, opBuilder);
    });

// Other services
builder.Services
    .AddScoped<IMovieRepository, MovieRepository>()
    .AddScoped<IUserService, UserService>()
    .AddSingleton<IDbExceptionsHelper, OracleDbExceptionsHelper>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Configuration.GetValue<string>("MigrateDatabases") is "YES")
    app.Services.MigrateDatabases();

// Startup cleaner  
app.Services.GetService<TokenListCleanerDemon>();
 
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();