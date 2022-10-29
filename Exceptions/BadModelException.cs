using System.Data.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MovieCatalogBackend.Exceptions;

public class BadModelException : DbException
{
    public ModelStateDictionary ModelState { get; set; } = new();
    public BadModelException(string message, Exception innerException) : base(message, innerException) { }
}