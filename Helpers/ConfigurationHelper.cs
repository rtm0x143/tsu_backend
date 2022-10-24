namespace MovieCatalogBackend.Helpers;

static public class ConfigurationHelper
{
    private static IConfiguration? _baseConfiguration = null;
    public static IConfiguration BaseConfiguration 
    { 
        get
        {
            if (_baseConfiguration == null) _baseConfiguration = CreateBaseConfiguration();
            return _baseConfiguration;
        }
        set => _baseConfiguration = value;
    }

    public static IConfiguration CreateBaseConfiguration()
    {
        var configurationBuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables();

        var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (envName != null)
            configurationBuilder.AddJsonFile(string.Format("appsettings.{0}.json", envName));

        return configurationBuilder.Build();
    }
}

