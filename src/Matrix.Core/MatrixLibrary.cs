namespace Matrix;

/// <param name="Companions">
/// Packages the library cannot run without but that are not its own primary package — for
/// example an abstractions or hosting package the primary package references without declaring as
/// a dependency. Declared once, on the module's <c>PackageReference</c>, via
/// <c>MatrixAotCompanion</c>. Empty for the ordinary library with no such gap.
/// </param>
public sealed record MatrixLibrary(
    string Id,
    string Name,
    string? Package,
    string? Version,
    bool Baseline,
    IReadOnlyList<MatrixPackage> Companions = default!)
{
    public IReadOnlyList<MatrixPackage> Companions { get; init; } = Companions ?? [];
}
