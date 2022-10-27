using System.ComponentModel.DataAnnotations.Schema;

namespace MovieCatalogBackend.Data;

public interface IHasGuid
{
    public Guid Id { get; }
}