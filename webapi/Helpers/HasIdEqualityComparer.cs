using MovieCatalogBackend.Data;
namespace MovieCatalogBackend.Helpers;

public class HasIdEqualityComparer : IEqualityComparer<IHasGuid>
{
    public HasIdEqualityComparer() { }
    public static HasIdEqualityComparer Instance = new();
    public bool Equals(IHasGuid? x, IHasGuid? y) => x?.Id == y?.Id;
    public int GetHashCode(IHasGuid obj) => obj.Id.GetHashCode();
}