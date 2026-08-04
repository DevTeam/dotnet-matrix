// ReSharper disable CheckNamespace
namespace Matrix.Web;

public interface IMatrixDataSource
{
    Task<MatrixCatalogResult> LoadCatalogAsync(CancellationToken cancellationToken = default);

    Task<DateTimeOffset?> LoadVersionDateAsync(
        MatrixWebCatalog catalog,
        MatrixVersion version,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CategoryReport>> LoadAsync(
        MatrixWebCatalog catalog,
        MatrixVersion version,
        IEnumerable<MatrixCategory> categories,
        CancellationToken cancellationToken = default);
}
