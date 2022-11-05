using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace MovieCatalogBackend.Services.Repositories;

public interface IRepository : IDisposable, IAsyncDisposable
{
    public DbContext InnerDbContext { get; }
    
    /// <remarks>
    /// see <see cref="DbContext"/> for exceptions 
    /// </remarks>
    public void FlushChanges();
    /// <remarks>
    /// see <see cref="DbContext"/> for exceptions 
    /// </remarks>
    public ValueTask FlushChangesAsync();
}