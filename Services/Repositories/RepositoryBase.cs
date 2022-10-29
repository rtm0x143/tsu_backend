using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace MovieCatalogBackend.Services.Repositories;

public abstract class RepositoryBase : IRepository
{
    protected bool IsDisposed;
    protected ILogger _logger;

    public ModelStateDictionary ModelState { get; } = new();
    public abstract DbContext InnerDbContext { get; }

    public RepositoryBase(ILogger logger) => _logger = logger;
    ~RepositoryBase() => Dispose();
    
    public virtual void Dispose()
    {
        if (IsDisposed == (IsDisposed = true)) return;
        try
        {
            FlushChanges();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unknown exception, while saving changes of {InnerDbContext.GetType()}");
        }
        finally
        {
            InnerDbContext.Dispose();
            GC.SuppressFinalize(this);   
        }
    }

    public virtual async ValueTask DisposeAsync()
    {
        if (IsDisposed == (IsDisposed = true)) return;
        try
        {
            await FlushChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unknown exception, while while saving changes of {InnerDbContext.GetType()}");
        }
        finally
        {
            await InnerDbContext.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }

    public virtual void FlushChanges() => InnerDbContext.SaveChanges();
    public virtual async ValueTask FlushChangesAsync() => await InnerDbContext.SaveChangesAsync();
}