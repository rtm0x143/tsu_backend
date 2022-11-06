namespace MovieCatalogBackend.Exceptions;

public interface IDbExceptionsHelper
{
    public bool IsAlreadyExist(Exception e);
    public bool IsNotFound(Exception e);
    
    public string Message { get; }
}