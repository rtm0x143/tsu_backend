using Microsoft.EntityFrameworkCore;

namespace MovieCatalogBackend.Helpers;

public static class ServiceProviderExtension
{
    public static void MigrateDatabases(this IServiceProvider services)
    {
        var assemblyTypes = System.Reflection.Assembly.GetExecutingAssembly().DefinedTypes.ToArray();
        foreach (var dbType in assemblyTypes.Where(t => t.IsSubclassOf(typeof(DbContext))))
            ((DbContext)services.GetService(dbType)!).Database.Migrate();
    }
}