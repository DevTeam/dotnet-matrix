namespace Matrix.Web;

/// <summary>
/// <paramref name="Warning"/> is set when the release list could not be read but the
/// application can still work with whatever versions it does have.
/// </summary>
public sealed record MatrixCatalogResult(
    MatrixWebCatalog Catalog,
    string? Warning);
