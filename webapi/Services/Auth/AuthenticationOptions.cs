using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MovieCatalogBackend.Services.Auth;

public class AuthenticationOptions
{
    public HashSet<string> Issuers { get; } = new();
    public HashSet<string> Audiences { get; } = new();
    public string? SecretKey { get; set; }
    public TimeSpan LiveTime { get; set; } = TimeSpan.FromHours(6.0d);

    public SymmetricSecurityKey? SecurityKey => 
        SecretKey != null 
            ? new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey)) 
            : null;

    public AuthenticationOptions() { }

    public AuthenticationOptions(IConfiguration configuration)
    {
        SecretKey = configuration.GetValue<string>("ServiceApiKey");
        Issuers =  configuration.GetValue<string[]>("ValidationParameters:Issuers")?.ToHashSet() ?? new();
        Audiences = configuration.GetValue<string[]>("ValidationParameters:Audiences")?.ToHashSet() ?? new();
    }

    public TokenValidationParameters CreateValidationParameters()
    {
        var parameters = new TokenValidationParameters { ValidateLifetime = true };
        parameters.ClockSkew = TimeSpan.Zero;
        
        if (parameters.ValidateIssuerSigningKey = SecretKey != null)
            parameters.IssuerSigningKey = SecurityKey;
        if (parameters.ValidateIssuer = Issuers.Count > 0)
            parameters.ValidIssuers = Issuers;
        if (parameters.ValidateAudience = Audiences.Count > 0) 
            parameters.ValidAudiences = Audiences;

        return parameters;
    }
}
