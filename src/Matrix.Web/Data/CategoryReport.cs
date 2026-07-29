namespace Matrix.Web;

public sealed record CategoryReport(
    MatrixCategory Category,
    FeatureReport? Features,
    BenchmarkReport? Benchmarks,
    MatrixLibraryMetadataCatalog? LibraryCatalog,
    MatrixChartCatalog? ChartCatalog,
    MatrixFeatureCatalog? FeatureCatalog,
    string? Error);
